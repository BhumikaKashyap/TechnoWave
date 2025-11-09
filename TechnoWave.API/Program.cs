
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;
using TechnoWave.Business.Repository.Interfaces;
using TechnoWave.Business.Repository.Services;
using TechnoWave.Infra.Context;

namespace TechnoWave.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add CORS (Allow Everything)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            // Configuration / Connection string
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

            // Add DbContext
            builder.Services.AddDbContext<TechnoWaveDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(180);
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                    }));
           

            // Add controllers
            builder.Services.AddControllers();

            // Add endpoints explorer required by Swagger
            builder.Services.AddEndpointsApiExplorer();

            // Configure JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)
                        ),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            builder.Configuration["Jwt:Key"] = key;
            builder.Services.AddAuthorization();

            // Register application services (DI)
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Swagger configuration (with JWT support)
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TechnoWave API",
                    Version = "v1"
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter 'Bearer {your token}'",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            // Configure the HTTP request pipeline.
            // Enable Swagger in Development and in Production if you want - here it's enabled for all envs.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechnoWave API v1");
                c.RoutePrefix = string.Empty; // serve Swagger at app root (optional). Remove to use /swagger
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
