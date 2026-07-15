using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserControl.Core.CustomEntities;
using UserControl.Core.Interfaces;
using UserControl.Core.Services;
using UserControl.Infrastructure.Data;
using UserControl.Infrastructure.Filters;
using UserControl.Infrastructure.Interfaces;
using UserControl.Infrastructure.Options;
using UserControl.Infrastructure.Repositories;
using UserControl.Infrastructure.Services;
using UserControl.Infrastructure.Validators;
using UserControl.Infrastructure.Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using UserControl.Api.Extensions;
using Microsoft.AspNetCore.Diagnostics;

namespace UserControl.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers(options =>
            {
                //SE COMENTA EL ANTERIOR PROCESO DE CONTROL DE EXCEPCIONES
               // options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(Options =>
            {
                //for ignore errors of
                Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //para ignorar variables que devuelve el response null
                Options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            })
            .ConfigureApiBehaviorOptions(options =>
            {
               // options.SuppressModelStateInvalidFilter = true;
            });

          
            services.AddDbContext<UserControlContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("SocialMedia"), ServerVersion.AutoDetect(Configuration.GetConnectionString("SocialMedia"))));


            services.AddOptions(Configuration);
            services.Configure<EmailSenderOptions>(Configuration.GetSection("EmailSenderOptions"));

            services.AddDbContextCustom(Configuration); //database
            services.AddServices();
            services.AddSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {

                // El API valida el JWT de Contexto
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });
       

            //definiendo los filtros de forma global
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(option =>
            
               
                option.RegisterValidatorsFromAssemblyContaining<PostValidator>()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureCustomExceptionMiddleware();


            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API");
                options.RoutePrefix = string.Empty;
            }
            );

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
