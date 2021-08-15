using System;

namespace Coop.Application.Advertisement
{
    public class AdvertisementListItemViewModel
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
        
        public string Text { get; set; }
        
        public Guid AuthorId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public bool IsPublished { get; set; } = false;
    }
}