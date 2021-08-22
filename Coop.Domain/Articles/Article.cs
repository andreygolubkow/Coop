using System;
using Ardalis.GuardClauses;
using Coop.Domain.Common;

namespace Coop.Domain.Articles
{
    public class Article : Entity
    {
        protected Article()
        {
        }

        public string Title { get; protected set; }

        public string Text { get; protected set; }

        public bool IsActive { get; protected set; }

        public Guid AuthorId { get; protected set; }

        /// <summary>
        ///     Создать новую статью(новость)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public static Article Create(string title, string text, Guid authorId)
        {
            Guard.Against.NullOrWhiteSpace(title, nameof(title), "Заголовок не может быть пустым");
            Guard.Against.NullOrWhiteSpace(text, nameof(text), "Текст не может быть пустым");
            Guard.Against.Default(authorId, nameof(authorId), "Необходимо указать пользователя - автора");

            return new Article
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                Title = title,
                Text = text,
                AuthorId = authorId
            };
        }

        /// <summary>
        ///     Обновить текст и заголовок.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        public void Update(string text, string title)
        {
            Guard.Against.NullOrWhiteSpace(text, nameof(text), "Текст не может быть пустым");
            Guard.Against.NullOrWhiteSpace(title, nameof(title), "Необходимо указать заголовок");
            if (!IsActive)
                throw new InvalidOperationException("Объект находиться в архиве, взаимодействовать с ним нельзя");

            Title = title;
            Text = text;
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        ///     Архивировать.
        /// </summary>
        public void Archive()
        {
            if (!IsActive) throw new InvalidOperationException("Объект уже в архиве");

            IsActive = false;
            UpdatedAt = DateTime.Now;
        }
    }
}