using BookStore.Application.Abstractions.Clock;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Exceptions;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Reviews;
using BookStore.Domain.Users;
using BookStore.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork, IApplicationDbContext
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IDateTimeProvider _dateTimeProvider;
        //private readonly IPublisher _publisher;

        public DbSet<Apartment> Apartments { get; private set; }

        public DbSet<Booking> Bookings { get; private set; }

        public DbSet<Review> Reviews { get; private set; }

        public DbSet<User> Users { get; private set; }


        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IDateTimeProvider dateTimeProvider
            //IPublisher publisher
            ) 
            : base(options)
        {
            // _publisher = publisher;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                AddDomainEventsAsOutboxMessages();

                var result = await base.SaveChangesAsync(cancellationToken);
                // await PublishDomainEventsAsync();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency exception occurred", ex);
            }
            
        }

        /* PublishDomainEvents
        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }
        */

        private void AddDomainEventsAsOutboxMessages()
        {
            var outboxMessages = ChangeTracker
                .Entries<IEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .Select(domainEvent => new OutboxMessage(
                    Guid.NewGuid(),
                    _dateTimeProvider.UtcNow,
                    domainEvent.GetType().Name,
                    JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
                .ToList();

            AddRange(outboxMessages);
        }
    }
}
