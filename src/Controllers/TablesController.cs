using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication.ClaimsAuthorizationLibrary;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Simple()
        {
            return View();
        }

        [ClaimsAuthorize(claimType = "WebApplication.Models.RegisterViewModel.UserType", claimValue = "manager")]
        public ActionResult Data()
        {
            try
            {
                IList<AnnouncementViewModel> announcementViewModels
                    = new List<AnnouncementViewModel>(100);

                IEnumerable<Announcement> announcements = db.Announcements.OrderByDescending(o => o.ID).ToArray();

                foreach (Announcement announcement in announcements)
                {
                    AnnouncementViewModel avm =
                        Mapper.Map<Announcement, AnnouncementViewModel>(announcement, opt =>
                                opt.BeforeMap((src, dest) => src.Comments = null));

                    if (announcement.AnnouncementReads == null)
                        announcement.AnnouncementReads = new List<AnnouncementRead>();

                    foreach (AnnouncementRead item in db.GetUsersNotinAnnouncemntRead(announcement))
                    {
                        AnnouncementReadViewModel i = Mapper.Map<AnnouncementRead, AnnouncementReadViewModel>(item);
                        i.UserEmail = item.UserName;
                        i.Status = "not seen";
                        avm.AnnouncementReads.Add(i);
                    }

                    announcementViewModels.Add(avm);
                }

                return View(announcementViewModels);
            }
            catch (Exception ex)
            {
                if (ModelState != null)
                    ModelState.AddModelError("Error", ex.Message);
                return View(new List<AnnouncementViewModel>());
            }
        }
    }
}