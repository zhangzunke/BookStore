﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Abstractions
{
    public abstract class Entity<TEntityId> : IEntity
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        protected Entity(TEntityId id) 
        {
            Id = id;
        }

        protected Entity()
        {
        }

        public TEntityId Id { get;  init; }
        public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
        public void ClearDomainEvents() => _domainEvents.Clear();
        protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }
}
