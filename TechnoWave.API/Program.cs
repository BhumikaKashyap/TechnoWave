
using Microsoft.EntityFrameworkCore;
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

            // Add services to the container.
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

            builder.Services.AddDbContext<TechnoWaveDbContext>(options =>
                options.UseSqlServer(
                   connectionString,
                   sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(180);
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null); // Optional: add specific error codes if needed
                    }));
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IAuthService, AuthService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
