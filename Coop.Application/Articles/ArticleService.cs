using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Coop.Application.Common;
using Coop.Domain.Articles;
using Coop.Domain.Common;

namespace Coop.Application.Articles
{
    public class ArticleService: IArticleService
    {
        private readonly IRepository<Article> _repository;
        private readonly IMapper _mapper;

        public ArticleService(IRepository<Article> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Create(CreateArticleInputModel model, Guid authorId, CancellationToken token)
        {
            var article = Article.Create(model.Title, model.Text, authorId);
            await _repository.AddAsync(article, token);
            var result = await _repository.SaveAsync(token);
            if (!result) throw new DatabaseException();
        }

        public UpdateArticleInputModel Get(Guid id)
        {
            var article = _repository.Find(id);
            if (article == null) return null;
            return _mapper.Map<Article, UpdateArticleInputModel>(article);
        }

        public async Task UpdateAsync(UpdateArticleInputModel model, CancellationToken token)
        {
            var article = _repository.Find(model.Id);
            Guard.Against.Null(article, "Не найдена новость");
            
            article.Update(model.Text, model.Title);
            _repository.Update(article);
            if (!await _repository.SaveAsync(token))
            {
                throw new DatabaseException();
            }
        }

        public async Task ArchiveAsync(Guid id, CancellationToken token)
        {
            var article = _repository.Find(id);
            Guard.Against.Null(article, "Не найдена новость");
            article.Archive();
            _repository.Update(article);
            if (!await _repository.SaveAsync(token))
            {
                throw new DatabaseException();
            }
        }

        public ArticleListViewModel GetPage(int page, int pageSize)
        {
            var articles = _repository.GetAll()
                .Where(a => a.IsActive)
                .OrderBy(a => a.CreatedAt);
            var count = articles.Count();
            return new ArticleListViewModel()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = count / pageSize + (count % pageSize == 0 ? 0 : 1),
                Items = articles
                    .Skip((page-1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<ArticleListItemViewModel>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }
    }
}