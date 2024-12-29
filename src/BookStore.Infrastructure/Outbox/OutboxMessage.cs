using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Outbox
{
    public sealed class OutboxMessage
    {
        public OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content) 
        {
            Id = id;
            OccurredOnUtc = occurredOnUtc;
            Content = content;
            Type = type;
        }

        public Guid Id { get; init; }
        public DateTime OccurredOnUtc { get; init; }
        public string Type { get; init; }
        public string Content { get; init; }
        public DateTime? ProcessedOnUtc { get; init; }
        public string? Error { get; init; }
    }
}
