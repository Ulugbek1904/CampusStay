using CampusStay.Brokers.Storages;
using CampusStay.DTO.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation.AspNetCore;
using FluentValidation;
using CampusStay.DTO.Users;
using CampusStay.Services.ValidationService;
using CampusStay.Services.UserServices;
using CampusStay.Brokers.Tokens;
using Serilog;
using System;
using CampusStay.Brokers.Email;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace CampusStay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();

                builder.Host.UseSerilog();

                Log.Information("Dastur ishga tushmoqda...");
                builder.Services.AddTransient<IStorageBroker, StorageBroker>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<ITokenBroker, TokenBroker>();
                builder.Services.AddScoped<IEmailBroker, EmailBroker>();

                builder.Services.AddAutoMapper(typeof(MappingProfile));
                builder.Services.AddControllers();

                builder.Services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();

                builder.Services.AddValidatorsFromAssemblyContaining<Program>();
                builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();

                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSwaggerGen(c =>
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT authorization using Bearer scheme. Example: \"Bearer {token}\"",
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


                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        NameClaimType = ClaimTypes.Email
                    };
                });

                builder.Services.AddAuthorization();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Dastur ishga tushirishda jiddiy xatolik yuz berdi.");
            }
            finally
            {
                Log.CloseAndFlush();
                Console.WriteLine("shu yerda tugayabdi");
            }
        }
    }
}
