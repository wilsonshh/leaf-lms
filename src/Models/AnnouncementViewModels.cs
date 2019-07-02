using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Extensions;

namespace WebApplication.Models
{
    public class AnnouncementViewModel : AnnouncementBase
    {
        public string Date
        {
            get
            {
                TimeSpan ts = DateTime.Now - this.CreateAt;

                if (ts < TimeSpan.FromHours(1))
                    return ts.Minutes + " mins ago";

                if (ts < TimeSpan.FromDays(1))
                    return ts.Hours + " hours ago";

                return ts.Days + " days ago";
            }
        }

        public string TruncatedBody
        {
            get
            {
                string s = Body?.ExtractHtmlInnerText() ?? string.Empty;
                if (s.Length < 33)
                    return s;
                else
                    return s.Substring(0, 30) + "...";
            }
        }

        public string TruncatedSubject
        {
            get
            {
                if (Subject?.Length < 33)
                    return Subject;
                else
                    return Subject.Substring(0, 30) + "...";
            }
        }

        public bool AtLeastOneread { get; set; }

        [NotMapped]
        public CommentViewModel Comment { get; set; } = new CommentViewModel();

        public List<AnnouncementReadViewModel> AnnouncementReads { get; set; }
    }

    public class AnnouncementReadViewModel : AnnouncementRead
    {
        public String Status { get; set; }
    }
}