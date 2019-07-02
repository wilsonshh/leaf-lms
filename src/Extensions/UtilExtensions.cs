using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using WebApplication.Models;

namespace WebApplication.Extensions
{
    public static class UtilExtensions
    {
        public static string ExtractHtmlInnerText(this string htmlText)
        {
            //Match any Html tag (opening or closing tags)
            // followed by any successive whitespaces
            //consider the Html text as a single line

            Regex regex = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);

            // replace all html tags (and consequtive whitespaces) by spaces
            // trim the first and last space

            string resultText = regex.Replace(htmlText, " ").Trim();

            return resultText;
        }

        public static SharedUnredAnnouncements GetPendingMessages(this IIdentity identity)
        {
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    string eMail = identity.GetUserEmail();

            //    return
            //        db.Announcements.Count(a => a.AnnouncementReads.FirstOrDefault(ar => ar.UserEmail == eMail) == null);
            //}

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                SharedUnredAnnouncements suAnnouncements = new SharedUnredAnnouncements();

                string userEmail = identity.GetUserEmail();

                IEnumerable<Announcement> unreadAnnouncements =
                (from announcement in db.Announcements
                 where announcement.AnnouncementReads.FirstOrDefault(u => u.UserEmail == userEmail) == null
                 select announcement);

                suAnnouncements.UnreadAnnouncements =
                    Mapper.Map<IEnumerable<Announcement>, IEnumerable<AnnouncementViewModel>>(unreadAnnouncements);

                //Performance - save the count, so you don´t have execute Count() method every time you need it
                suAnnouncements.Count = suAnnouncements.UnreadAnnouncements.Count();

                return suAnnouncements;
            }
        }
    }
}