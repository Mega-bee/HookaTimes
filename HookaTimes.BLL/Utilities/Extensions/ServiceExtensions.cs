﻿using HookaTimes.DAL.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Utilities.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<HookaTimes.DAL.Data.HookaDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            ;
        }

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication().AddJwtBearer(options =>
            {

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //Issuer is the API client 
                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:44310",

                    //Audience is the client 
                    ValidateAudience = true,
                    ValidAudience = "https://localhost:44310",

                    ValidateIssuerSigningKey = true,
                    //Our Secret Key
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fdjfhjehfjhfuehfbhvdbvjjoq8327483rgh")),

                    //for delaying the expiry  date of the token
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        var accessToken = context.Request.Query["access_token"];

                //        var path = context.HttpContext.Request.Path;
                //        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                //        {
                //            context.Token = accessToken;
                //        }

                //        return Task.CompletedTask;
                //    }
                //};
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Path.Value.StartsWith("/hubs/notification") &&
                            context.Request.Query.TryGetValue("token", out StringValues token)
                        )
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var te = context.Exception;
                        return Task.CompletedTask;
                    }
                };

            });
        }


        public static void ConfigureAuthenticationMVC(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                     .AddCookie(options =>
                     {
                         options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                         options.SlidingExpiration = true;
                         options.AccessDeniedPath = "/Forbidden/";
                         options.LoginPath = "/Account/Login";
                         //options.LogoutPath = "/Account/Logout";
                         options.LogoutPath = "/Home";
                         options.AccessDeniedPath = "/Account/AccessDenied";

                     });


        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HookaTimes", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
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
        }
    }
}
