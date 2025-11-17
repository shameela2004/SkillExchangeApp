using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp1.Application.Common.Mappings;
using MyApp1.Application.DependencyInjection;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Application.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using MyApp1.Infrastructure.Data;
using MyApp1.Infrastructure.Helpers;
using MyApp1.Infrastructure.RazorPay;
using MyApp1.Infrastructure.Repositories;
using MyApp1.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyApp1DbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("MyApp1.Infrastructure")));

            services.Configure<RazorpaySettings>(configuration.GetSection("RazorpaySettings"));
            services.AddScoped<RazorpayService>();


            // Registering Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            // Registering Services
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailSenderService, EmailSenderSevice>();
            //services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IConnectionService, ConnectionService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserLanguageService, UserLanguageService>();
            services.AddScoped<IUserSkillService, UserSkillService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IGroupSessionService, GroupSessionService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IUserReportService, UserReportService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IMessageService, MessageService>();




            return services;
        }
    }
}
