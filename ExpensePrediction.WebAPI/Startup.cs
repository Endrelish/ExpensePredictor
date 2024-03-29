﻿using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.BusinessLogicLayer.Mapper;
using ExpensePrediction.BusinessLogicLayer.Services;
using ExpensePrediction.DataAccessLayer;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace ExpensePrediction.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void SetUpDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                if (Configuration["UseLocalDb"].Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:Local"],
                        b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
                }
                else
                {
                    optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:Remote"],
                        b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
                }

                optionsBuilder
                    .UseLazyLoadingProxies()
                    .EnableSensitiveDataLogging();
            });
        }

        private void SetUpSecurity(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
                {
                    var pass = options.Password;
                    pass.RequireDigit = true;
                    pass.RequiredLength = 8;
                    pass.RequireLowercase = true;
                    pass.RequireNonAlphanumeric = true;
                    pass.RequireUppercase = true;
                }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear(); //remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = "identityRoles",
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddAuthorization(cfg =>
            {
                var policies = Configuration.GetSection("Policies").AsEnumerable(true)
                    .ToDictionary(p => p.Key, p => p.Value.Split(';'));
                foreach (var policy in policies)
                {
                    cfg.AddPolicy(policy.Key, p => p.RequireRole(policy.Value));
                }
            });
        }

        public void SetUpLogging(IServiceCollection services)
        {
            services.AddLogging(builder =>
               builder.AddConsole()
               .AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
            );
        }

        private void AddServices(IServiceCollection services)
        {
            //===============================Services===============================
            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient(typeof(IApplicationRepository<>), typeof(ApplicationRepository<>));

            services.AddTransient(typeof(ICategoryService<>), typeof(CategoryService<>));
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IExpenseService, ExpenseService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IIncomeService, IncomeService>();
            services.AddTransient<IPredictionService, PredictionService>();
            services.AddHostedService<HostedRegressionService>();

            services.AddSingleton(MapperService.Mapper);
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Inzynierka API", Version = "v1" });
                c.IncludeXmlComments(string.Format(@"{0}\SwaggerApiDescription.xml",
                    AppDomain.CurrentDomain.BaseDirectory));
                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter JWT with Bearer into field",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", Enumerable.Empty<string>()}
                });
            });
        }

        public void UseGlobalExceptionHandler(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            SetUpDbContext(services);
            SetUpSecurity(services);
            SetUpLogging(services);
            AddServices(services);
            AddSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            UseGlobalExceptionHandler(app);

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            dbContext.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inzynierka");
                c.RoutePrefix = "";
            });
        }
    }
}