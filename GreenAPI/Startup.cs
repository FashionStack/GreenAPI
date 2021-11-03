using FluentValidation;
using FluentValidation.AspNetCore;
using GreenAPI.Context;
using GreenAPI.Models.Validators;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using GreenAPI.Services;

namespace GreenAPI
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
            string stringDeConexao = Configuration.GetConnectionString("ConexaoMySQL");

            services.AddDbContext<GreenStockContext>(opt => opt.UseMySql(stringDeConexao, ServerVersion.AutoDetect(stringDeConexao))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableDetailedErrors()
                );

            services.AddControllers()
                            .AddFluentValidation(s =>
                             {
                                 s.RegisterValidatorsFromAssemblyContaining<Startup>();
                             });

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>());


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "GreenStock API",
                    Version = "v1",
                    Description = "Documentação referente ao software de controle de estoque GreenStock.<br /><br />" +
                    "Tal documentação relata todos os endpoints disponíveis para consumo, bem como seus devidos retornos.<br /><br />" +
                    "Desenvolvido pela equipe FashionStack.<br /><br />" +
                    "Quaisquer dúvidas, entre em contato conosco através do GitHub.<br /><br />",
                    Contact = new OpenApiContact
                    {
                        Name = "FashionStack",
                        Url = new Uri("https://github.com/FashionStack"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddFluentValidationRulesToSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GreenStock API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DatabaseManagementService.MigrationInitialization(app);
        }
    }
}
