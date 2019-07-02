using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public interface IAnnouncement<T> where T : IComment, new()
    {
        int ID { get; set; }
        string From { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        DateTime CreateAt { get; set; }
        ICollection<Comment> Comments { get; set; }
    }
}