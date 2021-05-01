using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mails;
using Optimisation;
using LinkedBack.Data;

namespace LinkedBack
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration conf)
        {
            Configuration = conf;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(opt =>
            {
                opt.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var secret = Encoding.ASCII.GetBytes(Configuration["Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var MailService = context.HttpContext.RequestServices.GetRequiredService<IUser>();
                        var MailId = int.Parse(context.Principal.Identity.Name);
                        var Mail = MailService.GetById(MailId);
                        if (Mail == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
               
                x.SaveToken = true;
               
                x.RequireHttpsMetadata = false;
                 x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IUser, UserDevices>();

            services.AddScoped<Emails, MailService>();

            services.AddSwaggerGen(swagger =>  
            {  
                swagger.SwaggerDoc("v1", new OpenApiInfo  
                {   
                    Version= "v1",   
                    Title = "Dorset College API",  
                    Description="ASP.NET Core 3.1 Web API Documentaion" 
                });
                swagger.AddSecurityDefinition("BEARER", new OpenApiSecurityScheme()  
                {  
                    Name = "Authorization",  
                    Type = SecuritySchemeType.ApiKey,  
                    Scheme = "BEARER",  
                    BearerFormat = "JWT",  
                    In = ParameterLocation.Header,  
                    Description = "JWT Authorization with the utilisation of BEARER. \r\n\r\n To proceed write 'BEARER' [space] and token in the text input.\r\n\r\nAn Exemple: \"BEARER 12345abcdef\"",  
                });  
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement  
                {  
                    {  
                        new OpenApiSecurityScheme  
                        {  
                            Reference = new OpenApiReference  
                            {  
                                Type = ReferenceType.SecurityScheme,  
                                Id = "BEARER"  
                            }  
                        },  
                        new string[] {}  
                    }  
                });  
            });  
        }

        
    }
}
