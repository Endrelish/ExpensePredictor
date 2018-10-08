using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthWebApi.Data;
using AuthWebApi.Data.Users.Entities;
using AuthWebApi.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace AuthWebApi
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
            services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                //optionsBuilder.UseSqlite("Data Source=db.db");
                optionsBuilder.UseSqlServer(
                    $"Server=tcp:expenses-prediction.database.windows.net,1433;" +
                    $"Initial Catalog=expenses-prediction;Persist Security Info=False;User ID=ppurgat;Password=zL7@5B*@!H;" +
                    $"MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
                cfg.AddPolicy("admin", p => p.RequireClaim("identityRoles", "admin"));
            });

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            var mappingConfig = new MapperConfiguration(mc => { mc.CreateMap<User, UserDataDto>(); });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "Inzynierka API", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            dbContext.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inzynierka"); });
        }
    }
}