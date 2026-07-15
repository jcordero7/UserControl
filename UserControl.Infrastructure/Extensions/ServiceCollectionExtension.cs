using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.DependencyInjection;
using UserControl.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;
using UserControl.Core.CustomEntities;
using UserControl.Infrastructure.Options;
using UserControl.Core.Interfaces;
using UserControl.Core.Services;
using UserControl.Infrastructure.Repositories;
using UserControl.Infrastructure.Interfaces;
using UserControl.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using UserControl.Application.Interfaces.services;
using UserControl.Application.Services;
using UserControl.Application.Interfaces;
//using AutoMapper.Configuration;

namespace UserControl.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDbContextCustom(this IServiceCollection services, IConfiguration configuration)
        {
            //el de MySql.Data.EntityFrameworkCore
            //services.AddDbContext<UserControlContext>(options =>
            //   options.UseMySql(configuration.GetConnectionString("SocialMedia"))
            //);


            services.AddDbContext<UserControlContext>(options =>
           options.UseMySql(configuration.GetConnectionString("oltp"), ServerVersion.AutoDetect(configuration.GetConnectionString("SocialMedia"))));


            //el de sql server 
            //options.UseSqlServer(Configuration.GetConnectionString("SocialMedia"))

            //--este es el de pomelo.Entity
            //options.UseMySql(Configuration.GetConnectionString("SocialMedia")) 
        }

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //esta funciona con la version 3.1.0 como esta en el startup.cs
            //services.Configure<PaginationOptions>(configuration.GetSection("Pagination"));
            //services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));

            //con la 3.1.8
            services.Configure<PaginationOptions>(options => configuration.GetSection("Pagination").Bind(options));
            services.Configure<PasswordOptions>(options => configuration.GetSection("PasswordOptions").Bind(options));
            
           // services.Configure<EmailSenderOptions>(options => configuration.GetSection("EmailSenderOptions"));
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //para resolver nuestras dependencias
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ISecurityService, SecurityService>();

            services.AddTransient<IUserService, UserService>();

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPasswordService, PasswordService>();

            services.AddTransient<ISendEmailService, SendEmailService>();

            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });

            return services;
        }


        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            //para resolver nuestras dependencias
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ISecurityService, SecurityService>();

            services.AddTransient<IUserService, UserService>();

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });

            return services;

        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFileName)
        {
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                doc.IncludeXmlComments(xmlPath);

            });

            return services;
        }



    }
}
