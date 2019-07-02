using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class Comment : IComment
    {
        [Key]
        [Display(AutoGenerateField = false)]
        public int ID { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string From { get; set; }

        [MaxLength(32767)]
        [Required]
        [AllowHtml]
        public string Text { get; set; }

        [Display(Name = "Sent At")]
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}