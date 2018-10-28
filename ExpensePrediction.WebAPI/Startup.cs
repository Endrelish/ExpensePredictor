using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.BusinessLogicLayer.Services;
using ExpensePrediction.DataAccessLayer;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Repositories;
using ExpensePrediction.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace ExpensePrediction.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LoggerFactory = new LoggerFactory(new[]
            {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information, true)
            });
        }

        public IConfiguration Configuration { get; }
        public readonly LoggerFactory LoggerFactory;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                //optionsBuilder.UseSqlite("Data Source=db.db");
                optionsBuilder.UseSqlServer(
                        $"Server=tcp:expenses-prediction.database.windows.net,1433;" +
                        $"Initial Catalog=expenses-prediction;Persist Security Info=False;User ID=ppurgat;Password=zL7@5B*@!H;" +
                        $"MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
                        b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName))
                    .UseLazyLoadingProxies()
                    .UseLoggerFactory(LoggerFactory)
                    .EnableSensitiveDataLogging();
            });

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    var pass = options.Password;
                    pass.RequireDigit = false;
                    pass.RequiredLength = 4;
                    pass.RequireLowercase = false;
                    pass.RequireNonAlphanumeric = false;
                    pass.RequireUppercase = false;
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
                        NameClaimType = "username",
                        RoleClaimType = "identityRoles",
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddAuthorization(cfg =>
            {
                var policies = Configuration.GetSection("Policies").AsEnumerable(makePathsRelative:true)
                    .ToDictionary(p => p.Key, p => p.Value.Split(';'));
                foreach (var policy in policies)
                {
                    cfg.AddPolicy(policy.Key, p => p.RequireRole(policy.Value));
                }
            });

            //===============================Services===============================
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped(typeof(IApplicationRepository<>), typeof(ApplicationRepository<>));
            services.AddScoped<IApplicationRepository<Expense>, ExpenseRepository>();
            services.AddScoped(typeof(ICategoryService<>), typeof(CategoryService<>));
            
            services.AddSingleton(MapperService.Mapper);


            //============================MVC and Swagger============================
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Inzynierka API", Version = "v1"});
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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