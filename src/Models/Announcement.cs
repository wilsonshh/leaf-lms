using System.Collections.Generic;

namespace WebApplication.Models
{
    public class Announcement : AnnouncementBase
    {
        public virtual ICollection<AnnouncementRead> AnnouncementReads { get; set; }
    }
}