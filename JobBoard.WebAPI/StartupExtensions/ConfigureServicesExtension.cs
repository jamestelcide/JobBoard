using JobBoard.Core.Domain.IdentityEntities;
using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Core.ServiceContracts;
using JobBoard.Core.Services;
using JobBoard.Infrastructure.DbContext;
using JobBoard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.WebAPI.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IJobListingRepository, JobListingRepository>();
            services.AddScoped<IJobListingService, JobListingService>();

            services.AddEndpointsApiExplorer(); //Generates description for all endpoints
            
            services.AddSwaggerGen(options => {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
            }); //generates OpenAPI specification

            services.AddCors(options => {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder
                    .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>())
                    .WithHeaders("Authorization", "origin", "accept", "content-type")
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    ;
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
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole,
                ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole,
                ApplicationDbContext, Guid>>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });

            services.AddControllers();

            return services;
        }
    }
}
