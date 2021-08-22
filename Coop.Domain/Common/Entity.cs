using System;

namespace Coop.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();

        /// <summary>
        ///     Используем DateTime, т.к. некоторые БД не работают с DTOffset
        /// </summary>
        public DateTime CreatedAt { get; protected set; } = DateTime.Now;

        public DateTime UpdatedAt { get; protected set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{GetType()}:{Id}";
        }
    }
}