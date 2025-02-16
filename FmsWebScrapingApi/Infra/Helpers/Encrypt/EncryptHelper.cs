using System.Security.Cryptography;
using System.Text;
using FmsWebScrapingApi.Infra.Config;

namespace FmsWebScrapingApi.Infra.Helpers.Encrypt
{
    public class EncryptHelper
    {
        private static string KeyEncrypt = String.Empty;
        private static string IVEncrypt = String.Empty;

        public static string EncryptString(string plainText)
        {
            IConfiguration configuration = AppSettingsConfig.GetConfiguration();
            KeyEncrypt = configuration[$"Encrypt:{configuration[$"Environment"]}:KeyEncrypt"];
            IVEncrypt = configuration[$"Encrypt:{configuration[$"Environment"]}:IV"];

            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            byte[] keyBytes = Encoding.UTF8.GetBytes(KeyEncrypt);
            byte[] ivBytes = Convert.FromBase64String(IVEncrypt);

            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            {
                throw new Exception($"O tamanho da chave de criptografia ({keyBytes.Length} bytes) é inválido. Deve ser 16, 24 ou 32 bytes.");
            }

            using var aesAlg = Aes.Create();
            aesAlg.Key = Encoding.ASCII.GetBytes(KeyEncrypt);
            aesAlg.IV = Convert.FromBase64String(IVEncrypt);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            var msEncrypt = new MemoryStream();
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                csEncrypt.Write(plainBytes, 0, plainBytes.Length);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string DecryptString(byte[] cipherText, byte[] key, byte[] IV)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = IV;

            try
            {
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using var msDecrypt = new MemoryStream(cipherText);

                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                string plaintext = srDecrypt.ReadToEnd();
                return plaintext;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
