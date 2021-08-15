using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Coop.Application.Articles
{
    public interface IArticleService
    {
        public ArticleListViewModel GetPage(int page, int pageSize);

        public Task Create(CreateArticleInputModel model, Guid authorId, CancellationToken token);

        public UpdateArticleInputModel Get(Guid id);
        Task UpdateAsync(UpdateArticleInputModel model, CancellationToken token);
        Task ArchiveAsync(Guid id, CancellationToken token);

    }
}