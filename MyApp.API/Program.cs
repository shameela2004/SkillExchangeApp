
using Microsoft.EntityFrameworkCore;
using MyApp1.Application.Interfaces.Repositories;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Application.Services;
using MyApp1.Infrastructure.Data;
using MyApp1.Infrastructure.Repositories;

namespace MyApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext
            builder.Services.AddDbContext<MyApp1DbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));
                


            // Registering Repositories
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Registering Services
            builder.Services.AddScoped(typeof(IGenericService<>),typeof(GenericService<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
