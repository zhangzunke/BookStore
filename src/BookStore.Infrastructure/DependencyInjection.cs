using BookStore.Application.Abstractions.Clock;
using BookStore.Application.Abstractions.Email;
using BookStore.Infrastructure.Clock;
using BookStore.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            var connectionString = configuration.GetConnectionString("Database") ??
                throw new ArgumentNullException(nameof(configuration));
            services.AddDbContext<ApplicationDbContext>(options => 
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
