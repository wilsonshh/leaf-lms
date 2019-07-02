using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public abstract class AnnouncementBase : IAnnouncement<Comment>
    {
        [Key]
        [Display(AutoGenerateField = false)]
        public int ID { get; set; }

        [Display(AutoGenerateField = false)]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string From { get; set; }

        [MaxLength(127)]
        [Column(TypeName = "VARCHAR")]
        [Required]
        public string Subject { get; set; }

        [MaxLength(32767)]
        [Required]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "Sent At")]
        public DateTime CreateAt { get; set; } = DateTime.Now;

        public virtual ICollection<Comment> Comments { get; set; }
    }
}