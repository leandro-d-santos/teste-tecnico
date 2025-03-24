
using Application.Clients.Services;
using Data.Clients.Repositories;
using Data.Connection;
using Domain.Clients.Repositories;
using Microsoft.Extensions.Configuration;

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


            app.MapControllers();

            app.Run();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            //services.AddScoped<IPedidoService, PedidoService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            //services.AddScoped<IPedidoRepository, PedidoRepository>();
        }
    }
}
