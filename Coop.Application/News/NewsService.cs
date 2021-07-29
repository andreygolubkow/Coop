using System;
using System.Collections.Generic;
using System.Linq;

namespace Coop.Application.News
{
    public class NewsService: INewsService
    {
        public static List<NewsListItemViewModel> TestNews = new List<NewsListItemViewModel>()
        {
            new NewsListItemViewModel()
            {
                Id = Guid.Parse("A3874A03-5DB1-45AC-A26E-4EBDAD7B58F2"),
                Title = "Технические работы",
                Details = "13 июня 2021 г. будет отключено электроснабжение с 11:00 до 12:00",
                CreatedAt = DateTimeOffset.Now.AddDays(-1)
            },
            new NewsListItemViewModel()
            {
                Id = Guid.Parse("10D339C7-6903-4B77-8885-2B62D2134CD3"),
                Title = "Задолженности",
                Details = "В связи с предстоящим ремонтом дороги просим всех оплатить членские взносы",
                CreatedAt = DateTimeOffset.Now.AddDays(-2)
            },
            new NewsListItemViewModel()
            {
                Id = Guid.Parse("48BAD6A6-507A-4803-9A94-BF4A122A0738"),
                Title = "Продажа гаражей",
                Details =
                    "В данный момент продаются гаражи 102а и 102б. По вопросам приобретения обращайтесь в администрацию кооператива.",
                CreatedAt = DateTimeOffset.Now.AddDays(-3)
            },
            new NewsListItemViewModel()
            {
                Id = Guid.Parse("A4A1CEA0-EC17-47DB-AF25-CF0A5BC8976E"),
                Title = "Новый способ оплаты",
                Details =
                    "Уважаемые члены кооператива, предлагаем вам вносить оплату с помощью QR-кода через приложение банка.",
                CreatedAt = DateTimeOffset.Now.AddDays(-4)
            },
            new NewsListItemViewModel()
            {
                Id = Guid.Parse("51982D9A-F557-46E8-A6D7-F3754E475422"),
                Title = "Новый сайт",
                Details =
                    "Добро пожаловать на новый сайт кооператива Моя Семья. Вопросы по использованию сайта вы можете задать администрации кооператива.",
                CreatedAt = DateTimeOffset.Now.AddDays(-5)
            }
        };
        
        public NewsListViewModel GetPage(int page, int pageSize)
        {
            return new NewsListViewModel()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = TestNews.Count / pageSize + (TestNews.Count % pageSize == 0 ? 0 : 1),
                Items = TestNews.Skip((page-1) * pageSize).Take(pageSize).ToList()
            };
        }
    }
}