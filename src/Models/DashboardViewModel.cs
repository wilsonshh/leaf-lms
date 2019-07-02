namespace WebApplication.Models
{
    public class DashboardViewModel
    {
        public int TotalUserUnreadMessages { get; set; }

        public int TotalRegistredUsers { get; set; }

        public AnnouncementViewModel Announcement { get; set; } = new AnnouncementViewModel();

        public int TotalSeenMessages { get; set; }

        public int TotalNotSeenMessages { get; set; }
    }
}