using System.Collections.Generic;

namespace WebApplication.Models
{
    public class SharedUnredAnnouncements
    {
        public int Count { get; set; }

        public IEnumerable<AnnouncementViewModel> UnreadAnnouncements { get; set; }
    }
}