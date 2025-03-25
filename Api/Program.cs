using Api.Core.Auth;
using Application.Clients.Services;
using Application.Orders.Services;
using Application.Tokens.Services;
using Data.Clients.Repositories;
using Data.Connection;
using Data.Orders.Repositories;
using Data.Tokens.Repositories;
using Domain.Clients.Repositories;
using Domain.Orders.Repositories;
using Domain.Tokens.Core;
using Domain.Tokens.Repositories;
using Microsoft.AspNetCore.Authentication;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton(new DbConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
            AddServices(builder.Services);
            AddRepositories(builder.Services);
            builder.Services.AddSingleton<TokenAuth>();
            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) 
                    .AllowCredentials());
            app.MapControllers();

            app.Run();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITokenSettingsService, TokenSettingsService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ITokenSettingsRepository, TokenSettingsRepository>();
            services.AddTransient<ITokenValidationRepository, TokenValidationRepository>();
        }
    }
}