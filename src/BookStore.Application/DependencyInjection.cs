using BookStore.Application.Abstractions.Behaviors;
using BookStore.Domain.Bookings;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace BookStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplications(this IServiceCollection services) 
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddTransient<PricingService>();

            return services;
        }
    }
}
