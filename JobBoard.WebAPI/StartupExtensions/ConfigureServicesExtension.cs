using JobBoard.Core.Domain.IdentityEntities;
using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Core.ServiceContracts;
using JobBoard.Core.Services;
using JobBoard.Infrastructure.DbContext;
using JobBoard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobBoard.WebAPI.StartupExtensions
{
    /// <summary>
    /// Extension methods for configuring services in the IServiceCollection
    /// This class contains methods to register various services, middleware, and configurations 
    /// needed for the application, including database context, repositories, identity services, 
    /// Swagger, CORS, and authentication settings.
    /// </summary>
    public static class ConfigureServicesExtension
    {
        /// <summary>
        /// Configures services for the application by setting up necessary dependencies and configurations.
        /// </summary>
        /// <param name="services">The IServiceCollection used to register application services.</param>
        /// <param name="configuration">The IConfiguration containing application configuration settings, such as connection strings and allowed origins.</param>
        /// <param name="env">The IHostEnvironment used to check the environment (Development, Test, etc.).</param>
        /// <returns>The <see cref="IServiceCollection"/> with configured services for dependency injection.</returns>
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IJobListingRepository, JobListingRepository>();
            services.AddScoped<IJobListingService, JobListingService>();
            services.AddTransient<IJwtService, JwtService>();

            services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new ConsumesAttribute("application/json"));

                //Authorization policy
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddXmlSerializerFormatters();

            services.AddEndpointsApiExplorer(); // Generates description for all endpoints
            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
            }); // generates OpenAPI specification

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

                    if (allowedOrigins != null && allowedOrigins.Length > 0)
                    {
                        policyBuilder.WithOrigins(allowedOrigins);
                    }
                    else
                    {
                        // Fallback if no allowed origins are provided
                        policyBuilder.WithOrigins("*"); // Allow all origins, or set to a default value
                    }

                    policyBuilder
                        .WithHeaders("Authorization", "origin", "accept", "content-type")
                        .WithMethods("GET", "POST", "PUT", "DELETE");
                });
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            //JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            services.AddAuthorization(options => {
            });

            services.AddControllers();

            return services;
        }
    }
}
