using System;

namespace Coop.Domain.Common
{
    public abstract class Entity
    {
        protected Entity(){}

        public Guid Id { get; protected set; } = Guid.NewGuid();
        
        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.Now;
        
        public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeOffset.Now;

        public override string ToString()
        {
            return $"{GetType()}:{Id}";
        }
    }
}