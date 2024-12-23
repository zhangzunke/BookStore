using BookStore.Application.Abstractions.Clock;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Abstractions.Email;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Users;
using BookStore.Infrastructure.Clock;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Email;
using BookStore.Infrastructure.Repositories;
using Dapper;
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

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IApartmentRepository, ApartmentRepository>();

            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ =>
                new SqlConnectionFactory(connectionString));

            SqlMapper.AddTypeHandler(new IntListHandler());

            return services;
        }
    }
}
