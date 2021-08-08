using System;
using Ardalis.GuardClauses;
using Coop.Domain.Common;

namespace Coop.Domain.Advertisements
{
    /// <summary>
    /// Рекламное объявление.
    /// </summary>
    public class Advertisement: Entity
    {
        protected Advertisement(){}
        
        public string Title { get; protected set; }
        
        public string Text { get; protected set; }
        
        /// <summary>
        /// Если не активно - то объявление в архиве.
        /// </summary>
        public bool IsActive { get; protected set; }
        
        /// <summary>
        /// Опубликовано или нет.
        /// </summary>
        public bool IsPublished { get; protected set; }

        /// <summary>
        /// Кто автор объявления.
        /// </summary>
        public Guid AuthorId { get; protected set; }
        
        /// <summary>
        /// Кто опубликовал объявление.
        /// </summary>
        public Guid? PublisherId { get; protected set; }

        public static Advertisement Create(string title, string text, Guid authorId)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title), "Необходимо указать заголовок объявления");
            Guard.Against.NullOrWhiteSpace(text, nameof(text), "Необходимо указать текст объявления");
            Guard.Against.Default(authorId, nameof(authorId), "Нельзя создать объявление без автора");

            return new Advertisement()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now,
                Title = title,
                Text = text,
                IsActive = true,
                IsPublished = false,
                AuthorId = authorId,
                PublisherId = null
            };
        }

        public void Change(string newTitle, string newText)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Объявление неактивно и изменить его нельзя");
            }
            if (IsPublished)
            {
                throw new InvalidOperationException("Объявление уже опубликовано и изменить его нельзя.");
            }

            Guard.Against.NullOrWhiteSpace(newText, nameof(newText), "Текст объявления не может быть пустым");
            Guard.Against.NullOrWhiteSpace(newTitle, nameof(newTitle), "Необходимо указать заголовок объявления");
            
            UpdatedAt = DateTimeOffset.Now;
            Text = newText;
            Title = newTitle;
        }

        public void Publish(Guid publisherId)
        {
            Guard.Against.Default(publisherId, nameof(publisherId),
                "Необходимо указать пользователя, ответственного за публикацию ");
            if (!IsActive)
            {
                throw new InvalidOperationException("Объявление в архиве и не может быть опубликовано");
            }

            if (IsPublished)
            {
                throw new InvalidOperationException("Объявление уже опубликовано");
            }

            PublisherId = publisherId;
            IsPublished = true;
            UpdatedAt = DateTimeOffset.Now;
        }

        public void Archive()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Объявление уже в архиве");
            }

            IsActive = false;
            UpdatedAt = DateTimeOffset.Now;
        }
    }
}