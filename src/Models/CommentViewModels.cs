using System;

namespace WebApplication.Models
{
    public class CommentReadOnlyViewModel : IComment
    {
        public string From { get; set; }
        public int ID { get; set; }

        public string Text { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
    }

    public class CommentViewModel : Comment
    {
    }
}