namespace Coop.Application.News
{
    public interface INewsService
    {
        public NewsListViewModel GetPage(int page, int pageSize);
    }
}