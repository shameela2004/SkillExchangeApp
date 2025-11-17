
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyApp1.API.Extensions;
using MyApp1.API.Hubs;
using MyApp1.Application.Common.Mappings;
using MyApp1.Application.DependencyInjection;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Application.Services;
using MyApp1.Domain.Interfaces;
using MyApp1.Infrastructure.Data;
using MyApp1.Infrastructure.DependencyInjection;
using MyApp1.Infrastructure.Helpers;
using MyApp1.Infrastructure.Repositories;
using MyApp1.Infrastructure.Seeders;
using System.IO;
using System.Text;

namespace MyApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");


            builder.Services.AddControllers();
            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;  // Adjust as per your needs
    });


            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            // Jwt Authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            // Use environment variable JWT_SECRET if present, else fallback to config file secret
            var secretKeyString = Environment.GetEnvironmentVariable("JWT_SECRET") ?? jwtSettings["SecretKey"];
            var secretKey = Encoding.UTF8.GetBytes(secretKeyString);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.Zero
                };
               
                // Extract token from cookie instead of Authorization header
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // previous correct working one ..
                        //var accessToken = context.Request.Cookies["accessToken"];
                        ////var path = context.HttpContext.Request.Path;
                        ////&& path.StartsWithSegments("/hubs/chat")
                        //if (!string.IsNullOrEmpty(accessToken) )
                        //{
                        //    context.Token = accessToken;
                        //}
                        //return Task.CompletedTask;



                        // 1️⃣ Always try to read token from cookie for REST APIs
                        var accessToken = context.Request.Cookies["accessToken"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                            return Task.CompletedTask;
                        }

                        // 2️⃣ SignalR: read token from query for websockets
                        var accessTokenQuery = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessTokenQuery) &&
                            path.StartsWithSegments("/hubs/chat"))
                        {
                            context.Token = accessTokenQuery;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
            builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddSignalR();
            builder.Services.AddScoped<INotificationSender, NotificationSender>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:5174", "http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });




            var app = builder.Build();


            app.UseGlobalExceptionHandler();
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MyApp1DbContext>();
                DataSeeder.SeedAdminUser(context).GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();


            // 🔥 Create voices folder inside wwwroot
            var env = app.Services.GetRequiredService<IWebHostEnvironment>();

            if (!Directory.Exists(env.WebRootPath))
            {
                Directory.CreateDirectory(env.WebRootPath);
            }

            var voicesPath = Path.Combine(env.WebRootPath, "voices");

            if (!Directory.Exists(voicesPath))
            {
                Directory.CreateDirectory(voicesPath);
            }
            app.UseStaticFiles();
            // Enable serving static files
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".webm"] = "audio/webm";

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.WebRootPath, "voices")
                ),
                RequestPath = "/voices",
                ContentTypeProvider = provider
            });


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHub<ChatHub>("/hubs/chat");
            //});
            app.MapHub<ChatHub>("/hubs/chat");
            app.MapHub<GroupChatHub>("/hubs/groupchat");



            app.Run();
        }
    }
}
