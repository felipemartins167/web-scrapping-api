using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Infra.Helpers.Encrypt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace FmsWebScrapingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("v1/check")]
        public async Task<IActionResult> CheckStatusApi()
        {
            IActionResult result = Ok();
            try
            {
                result = Ok(new ApiResponse<dynamic>(new { Version = "0.0.1" }, false, null, null, null));
            }
            catch (AuthException ex)
            {
                result = StatusCode(401, new ApiResponse<AuthException>(ex, true, ex.ErrorMessage, null, null));
            }
            catch (ApiException ex)
            {
                result = StatusCode(400, new ApiResponse<ApiException>(ex, true, ex.ErrorMessage, null, null));
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ApiResponse<Exception>(ex, true, ex.Message, null, null));
            }
            return result;
        }

        /// <summary>
        /// Endpoint de diagnóstico/eco que retorna todas as informações da requisição,
        /// incluindo certificado de cliente (se enviado) e detalhes TLS.
        /// Aceita GET/HEAD/POST/PUT/PATCH/DELETE/OPTIONS.
        /// </summary>
        [AllowAnonymous]
        [AcceptVerbs("GET", "HEAD", "POST", "PUT", "PATCH", "DELETE", "OPTIONS")]
        [Route("v1/echo")]
        [RequestSizeLimit(50_000_000)] // opcional: limite de 50MB para corpo e arquivos
        public async Task<IActionResult> EchoAll()
        {
            try
            {
                var ctx = HttpContext;
                var req = ctx.Request;

                // --- HEADERS, COOKIES, QUERY, ROUTE, CLAIMS ---
                var headers = req.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
                var cookies = req.Cookies.ToDictionary(c => c.Key, c => c.Value);
                var query = req.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
                var routeValues = req.RouteValues.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString());
                var claims = ctx.User?.Claims?.Select(c => new
                {
                    c.Type,
                    c.Value,
                    c.ValueType,
                    c.Issuer,
                    c.OriginalIssuer
                }).ToList();

                // --- FORM E ARQUIVOS (multipart/x-www-form-urlencoded) ---
                Dictionary<string, string[]>? formFields = null;
                var files = new List<object>();
                if (req.HasFormContentType)
                {
                    var form = await req.ReadFormAsync();
                    formFields = form
                        .Where(kv => kv.Value.Count > 0)
                        .ToDictionary(kv => kv.Key, kv => kv.Value.ToArray());

                    foreach (var f in form.Files)
                    {
                        await using var fs = f.OpenReadStream();
                        using var ms = new MemoryStream();
                        await fs.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();

                        files.Add(new
                        {
                            f.Name,
                            f.FileName,
                            f.ContentType,
                            f.Length,
                            Base64 = Convert.ToBase64String(fileBytes)
                        });
                    }
                }

                // --- CORPO BRUTO (qualquer content-type) ---
                req.EnableBuffering(); // permite ler o Body sem consumi-lo
                byte[] bodyBytes;
                string? bodyText;
                using (var ms = new MemoryStream())
                {
                    await req.Body.CopyToAsync(ms);
                    bodyBytes = ms.ToArray();
                }
                req.Body.Position = 0; // reseta o stream p/ middlewares posteriores, se existirem
                bodyText = TryGetText(bodyBytes, req.ContentType);

                // --- TLS/HANDSHAKE E CERTIFICADO DE CLIENTE ---
                var tls = ctx.Features.Get<ITlsHandshakeFeature>();
                var clientCert = ctx.Connection.ClientCertificate
                                ?? await ctx.Connection.GetClientCertificateAsync();

                object? certificateDto = null;
                if (clientCert is not null)
                {
                    certificateDto = new
                    {
                        Subject = clientCert.Subject,
                        Issuer = clientCert.Issuer,
                        NotBefore = clientCert.NotBefore,
                        NotAfter = clientCert.NotAfter,
                        Thumbprint = clientCert.Thumbprint,
                        SerialNumber = clientCert.SerialNumber,
                        SignatureAlgorithm = clientCert.SignatureAlgorithm.FriendlyName,
                        HasPrivateKey = clientCert.HasPrivateKey,
                        PublicKeyAlgorithm = clientCert.PublicKey.Oid?.FriendlyName,
                        RawDataBase64 = Convert.ToBase64String(clientCert.RawData)
                        // Observação: SANs e cadeia completa exigem parsing extra/validação externa.
                    };
                }

                var responsePayload = new
                {
                    Request = new
                    {
                        Method = req.Method,
                        Scheme = req.Scheme,
                        Host = req.Host.Value,
                        Path = req.Path.Value,
                        QueryString = req.QueryString.Value,
                        Protocol = req.Protocol,
                        ContentType = req.ContentType,
                        ContentLength = req.ContentLength
                    },
                    Connection = new
                    {
                        LocalIp = ctx.Connection.LocalIpAddress?.ToString(),
                        LocalPort = ctx.Connection.LocalPort,
                        RemoteIp = ctx.Connection.RemoteIpAddress?.ToString(),
                        RemotePort = ctx.Connection.RemotePort,
                        Tls = tls is null ? null : new
                        {
                            Protocol = tls.Protocol.ToString(),
                            CipherAlgorithm = tls.CipherAlgorithm.ToString(),
                            tls.CipherStrength,
                            HashAlgorithm = tls.HashAlgorithm.ToString(),
                            tls.HashStrength,
                            KeyExchangeAlgorithm = tls.KeyExchangeAlgorithm.ToString(),
                            tls.KeyExchangeStrength
                        },
                        ClientCertificate = certificateDto
                    },
                    Headers = headers,
                    Cookies = cookies,
                    Query = query,
                    RouteValues = routeValues,
                    Claims = claims,
                    Form = formFields,
                    Files = files,
                    Body = new
                    {
                        Text = bodyText,
                        Base64 = bodyBytes.Length > 0 ? Convert.ToBase64String(bodyBytes) : null
                    }
                };

                return Ok(new ApiResponse<dynamic>(responsePayload, false, null, null, null));
            }
            catch (AuthException ex)
            {
                return StatusCode(401, new ApiResponse<AuthException>(ex, true, ex.ErrorMessage, null, null));
            }
            catch (ApiException ex)
            {
                return StatusCode(400, new ApiResponse<ApiException>(ex, true, ex.ErrorMessage, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Exception>(ex, true, ex.Message, null, null));
            }
        }

        // Tenta renderizar o corpo como texto se o content-type indicar texto/JSON/XML/etc.
        private static string? TryGetText(byte[] bytes, string? contentType)
        {
            if (bytes is null || bytes.Length == 0) return null;

            bool IsLikelyText(string? ct) =>
                !string.IsNullOrWhiteSpace(ct) &&
                (ct.StartsWith("text/", StringComparison.OrdinalIgnoreCase)
                 || ct.Contains("json", StringComparison.OrdinalIgnoreCase)
                 || ct.Contains("xml", StringComparison.OrdinalIgnoreCase)
                 || ct.Contains("yaml", StringComparison.OrdinalIgnoreCase)
                 || ct.Contains("x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase));

            if (!IsLikelyText(contentType)) return null;

            var enc = Encoding.UTF8;
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                var m = Regex.Match(contentType, "charset=([^;]+)", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    try { enc = Encoding.GetEncoding(m.Groups[1].Value.Trim()); } catch { /* fallback UTF-8 */ }
                }
            }

            return enc.GetString(bytes);
        }
    }
}
