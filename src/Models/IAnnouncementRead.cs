using System;

namespace WebApplication.Models
{
    public interface IAnnouncementRead
    {
        /// <summary>
        /// Id dessa entidade
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// Name of the  Name who read the Announcement
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Email of the user who read the Announcement
        /// </summary>
        string UserEmail { get; set; }

        /// <summary>
        /// Date Time when the Annoucnement was Read
        /// </summary>
        DateTime DateTimeRead { get; set; }
    }
}