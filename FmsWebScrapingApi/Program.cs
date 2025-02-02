
using System.Text;
using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Repository.EmailRepositoryHandler;
using FmsWebScrapingApi.Data.Repository.ProductRepositoryHandler;
using FmsWebScrapingApi.Data.Repository.SettingsRepositoryHandler;
using FmsWebScrapingApi.Data.Repository.UserRepositoryHandler;
using FmsWebScrapingApi.Services.Implementations;
using FmsWebScrapingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add repositories to the container
        builder.Services.TryAddScoped<IUserRepository, UserRepository>();
        builder.Services.TryAddScoped<IEmailRepository, EmailRepository>();
        builder.Services.TryAddScoped<ISettingsRepository, SettingsRepository>();
        builder.Services.TryAddScoped<IProductRepository, ProductRepository>();

        // Add services to the container.
        builder.Services.TryAddScoped<IAuthService, AuthService>();
        builder.Services.TryAddScoped<IUserService, UserService>();
        builder.Services.TryAddScoped<IProductService, ProductService>();

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddControllers().AddNewtonsoftJson();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Configuration.AddJsonFile("appsettings.json");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAllHeaders");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}