using Asp.Versioning;
using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Caching;
using BookStore.Application.Abstractions.Clock;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Abstractions.Email;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Reviews;
using BookStore.Domain.Users;
using BookStore.Infrastructure.Authentication;
using BookStore.Infrastructure.Authorization;
using BookStore.Infrastructure.Caching;
using BookStore.Infrastructure.Clock;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Email;
using BookStore.Infrastructure.HealthChecks;
using BookStore.Infrastructure.Outbox;
using BookStore.Infrastructure.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using AuthenticationOptions = BookStore.Infrastructure.Authentication.AuthenticationOptions;
using AuthenticationService = BookStore.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = BookStore.Application.Abstractions.Authentication.IAuthenticationService;

namespace BookStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            AddPersistence(services, configuration);
            AddCaching(services, configuration);
            AddAuthentication(services, configuration);
            AddAuthorization(services);
            AddHealthChecks(services, configuration);
            AddApiVersioning(services);
            AddBackgroundJobs(services, configuration);
            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database") ??
               throw new ArgumentNullException(nameof(configuration));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IApartmentRepository, ApartmentRepository>();

            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ =>
                new SqlConnectionFactory(connectionString));

            SqlMapper.AddTypeHandler(new IntListHandler());
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();

            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
            services.ConfigureOptions<JwtBearerOptionsSetup>();
            services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

            services.AddTransient<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
            {
                var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
           .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) => 
            {
                var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
        }

        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddScoped<AuthorizationService>();
            services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        }

        private static void AddCaching(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Cache") ??
                throw new ArgumentNullException(nameof(configuration));
            services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);
            services.AddSingleton<ICacheService, CacheService>();
        }

        private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                var keycloakUrl = configuration["Keycloak:BaseUrl"]!;
                return new KeycloakHealthCheck(httpClient, keycloakUrl);
            });

            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!)
                .AddRedis(configuration.GetConnectionString("Cache")!)
                .AddCheck<KeycloakHealthCheck>("keycloak", timeout: TimeSpan.FromSeconds(30));
                //.AddUrlGroup(new Uri(configuration["Keycloak:BaseUrl"]!), HttpMethod.Get, "keycloak", timeout: TimeSpan.FromSeconds(60));
        }

        private static void AddApiVersioning(IServiceCollection services)
        {
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
            services.AddQuartz();
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
            services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
        }
    }
}
