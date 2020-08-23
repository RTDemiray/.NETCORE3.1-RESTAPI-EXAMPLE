using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlutterApp.Api.Extensions;
using FlutterApp.Api.Filters;
using FlutterApp.Api.Helpers;
using FlutterApp.Api.Services;
using FlutterApp.Core.IRepositories;
using FlutterApp.Core.Repositories;
using FlutterApp.Data.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FlutterApp.Api
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
            // AutoMapper kütüphanesinin register iþlemi.
            services.AddAutoMapper(typeof(Startup));

            // özel anahtar yapýlandýrýlmasý.
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Jwt authentication yapýlandýrýlmasý.
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Cors yapýlandýrýlmasý.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            // Swagger kütüphanesinin register edilmesi.
            services.AddSwaggerGen(gen =>
            {
                gen.SwaggerDoc("FlutterAppAPIV1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "FlutterApp API",
                    Description = "Flutter uygulamasý için yazýlmýþ api uygulamasýdýr.",
                    Contact = new OpenApiContact
                    {
                        Name = "Recep Tayyip DEMÝRAY",
                        Email = "recep_tayyip_demiray@outlook.com"
                    }
                });

                gen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Lütfen JWT Bearer token giriniz!",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                gen.AddSecurityRequirement(new OpenApiSecurityRequirement {
                 {
                   new OpenApiSecurityScheme
                   {
                     Reference = new OpenApiReference
                     {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                     }
                    },
                    new string[] { }
                  }
                });

                // Kaydedilen xml üzerinden verilerin okunup swagger'ýn oluþturulmasý.
                var xmlFiles = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFiles);
                gen.IncludeXmlComments(xmlPath);
            });

            // configure DI for application services
            // Dependency Injection yapýlandýrýlmasý.
            services.AddScoped(typeof(NotFoundFilter<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //ConnectionString'in Manage User Secrets üzerinden okunmasý.
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection"))
            );

            services.AddControllers(o => o.Filters.Add(new ValidationFilter()))
        .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new IntToStringConverter()));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Cors middleware eklenmesi.
            app.UseCors("AllowMyOrigin");

            // Swagger middleware eklenmesi.
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/FlutterAppAPIV1/swagger.json", "FlutterApp API");
            });

            // Exeception middleware eklenmesi.
            app.UseCustomException();

            app.UseHttpsRedirection();

            // Route attribute middleware eklenmesi.
            app.UseRouting();

            // Authentice,Authorize middleware eklenmesi.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
