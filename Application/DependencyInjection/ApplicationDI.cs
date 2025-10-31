using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp1.Application.Common.Mappings;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DependencyInjection
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register only interfaces here, implementations are in Infrastructure
            services.AddScoped<SocialTextParser>();

            // Register AutoMapper profiles from Application assembly if you keep maps here
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
