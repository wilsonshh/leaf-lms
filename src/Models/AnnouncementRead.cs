using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class AnnouncementRead : IAnnouncementRead
    {
        /// <summary>
        /// Id dessa entidade
        /// </summary>
        [Key]
        [Display(AutoGenerateField = false)]
        public int ID { get; set; }

        /// <summary>
        /// Name of the  Name who read the Announcement
        /// </summary>
        [Display(Name = "User Name")]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string UserName { get; set; }

        /// <summary>
        /// Email of the user who read the Announcement
        /// </summary>
        [Display(Name = "User E-mail")]
        [Index]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string UserEmail { get; set; }

        /// <summary>
        /// Date Time when the Annoucnement was Read
        /// </summary>
        [Display(Name = "Read At")]
        public DateTime DateTimeRead { get; set; } = DateTime.Now;
    }
}