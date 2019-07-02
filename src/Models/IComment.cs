using System;

namespace WebApplication.Models
{
    public interface IComment
    {
        int ID { get; set; }
        string From { get; set; }
        string Text { get; set; }

        DateTime CreateAt { get; set; }
    }
}