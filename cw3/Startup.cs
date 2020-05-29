using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using cw3.Middlewares;
using cw3.Services.EncryptionService;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace cw3
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  var secret = Environment.GetEnvironmentVariable("JWT__KEY");
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidIssuer = "https://localhost",
                      ValidAudience = "https://localhost",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                  };
              });
            services.AddScoped<IDbService, DbService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddControllers();
            services.AddDataProtection();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDeveloperExceptionPage();

            app.UseMiddleware<LoggingMiddleware>();

            app.Use(async (context, next) => {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Index of the user should be set in header");
                    return;
                }

                var index = context.Request.Headers["Index"].ToString();

                DbService studentDbService = new DbService();

                if (!studentDbService.IsExistingStudent(index))
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Index does not exist in database");
                    return;
                }
                await next();
            });

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();
            
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("custom", "secret123");
                await next();
                
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
