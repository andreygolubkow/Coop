namespace Coop.Application.Articles
{
    public interface IArticleService
    {
        public ArticleListViewModel GetPage(int page, int pageSize);
    }
}