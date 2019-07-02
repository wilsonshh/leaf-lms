using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.ClaimsAuthorizationLibrary;
using WebApplication.Extensions;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class MailboxController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Inbox()
        {
            try
            {
                IEnumerable<Announcement> announcements =
                    await db.Announcements.OrderByDescending(m => m.CreateAt).ToArrayAsync();

                IList<AnnouncementViewModel> evmList =
                    new List<AnnouncementViewModel>(100);

                //For performance reasons excludes unecessary
                // Comments and Announcements from Mapping
                foreach (var avm in announcements)
                {
                    AnnouncementViewModel evm =
                        Mapper.Map<Announcement, AnnouncementViewModel>(avm, opt =>
                            opt.BeforeMap((src, dest) =>
                                {
                                    dest.AtLeastOneread = (src.AnnouncementReads.Count > 0) ? true : false;
                                    src.Comments = null;
                                    src.AnnouncementReads = null;
                                }
                            ));
                    evmList.Add(evm);
                }

                return View(evmList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return View(new AnnouncementViewModel[0]);
        }

        public async Task<ActionResult> AnnouncementReadStatus(int? id)
        {
            try
            {
                Announcement announcement = null;

                if (id == null)
                    announcement = db.Announcements.OrderByDescending(a => a.CreateAt).FirstOrDefault();
                else
                    announcement = await db.Announcements.FindAsync(id);

                if (announcement == null)
                    throw new ApplicationException("Announcement does not exists");

                AnnouncementViewModel avm =
                    Mapper.Map<Announcement, AnnouncementViewModel>(announcement);

                if (announcement.AnnouncementReads == null)
                    announcement.AnnouncementReads = new List<AnnouncementRead>();

                foreach (AnnouncementRead item in db.GetUsersNotinAnnouncemntRead(announcement))
                {
                    AnnouncementReadViewModel i = Mapper.Map<AnnouncementRead, AnnouncementReadViewModel>(item);
                    i.Status = "Not seen";
                    avm.AnnouncementReads.Add(i);
                }

                return View(avm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(new AnnouncementViewModel());
            }
        }

        [ClaimsAuthorize(claimType = "WebApplication.Models.RegisterViewModel.UserType", claimValue = "manager")]
        public ActionResult Compose()
        {
            if (!User.Identity.IsManager())
                throw new SecurityException();

            return View();
        }

        // POST: AnnouncementViewModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(claimType = "WebApplication.Models.RegisterViewModel.UserType", claimValue = "manager")]
        public async Task<ActionResult> Compose([Bind(Include = "ID,Subject,Body")] AnnouncementViewModel announcementViewModelModel)
        {
            try
            {
                if (!User.Identity.IsManager())
                    throw new SecurityException();

                if (ModelState.IsValid)
                {
                    ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

                    announcementViewModelModel.From = user.NameIdentifier;
                    Announcement announcement =
                        Mapper.Map<AnnouncementViewModel, Announcement>(announcementViewModelModel);

                    db.Announcements.Add(announcement);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Inbox");
                }
            }
            catch (Exception ex)
            {
                //TODO: Insert Error Field on Compose View
                ModelState.AddModelError("Error", ex.Message);
            }
            return View(announcementViewModelModel);
        }

        public async Task<ActionResult> Read(int? id)
        {
            try
            {
                Announcement announcement = null;

                if (id == null)
                    announcement = db.Announcements.OrderByDescending(a => a.CreateAt).FirstOrDefault();
                else
                    announcement = await db.Announcements.FindAsync(id);

                if (announcement == null)
                    throw new ApplicationException("Announcement does not exists");

                AnnouncementViewModel avm =
                    Mapper.Map<Announcement, AnnouncementViewModel>(announcement);

                if (announcement.AnnouncementReads == null)
                    announcement.AnnouncementReads = new List<AnnouncementRead>();

                await SaveAnnouncementReadInformation(announcement);

                return View(avm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(new AnnouncementViewModel());
            }
        }

        private async Task SaveAnnouncementReadInformation(Announcement announcement)
        {
            string userEmail = User.Identity.GetUserEmail();
            string userName = User.Identity.GetUserNameIdentifier();

            AnnouncementRead ar =
                announcement.AnnouncementReads.FirstOrDefault(x => x.UserEmail == userEmail);

            if (ar == null)
            {
                ar = new AnnouncementRead()
                {
                    UserEmail = userEmail,
                    UserName = userName,
                    DateTimeRead = DateTime.Now,
                };
                announcement.AnnouncementReads.Add(ar);
                await db.SaveChangesAsync();
            }
        }

        // POST: CommentViewModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Read(AnnouncementViewModel announcementViewModelModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comment =
                        Mapper.Map<CommentViewModel, Comment>(announcementViewModelModel.Comment);

                    ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                    comment.From = user.NameIdentifier;

                    Announcement announcement = await db.Announcements.FindAsync(announcementViewModelModel.ID);

                    if (announcement.Comments == null)
                        announcement.Comments = new List<Comment>(1);

                    announcement.Comments.Add(comment);

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                //TODO: Insert Error Field on Compose View
                ModelState.AddModelError("Error", ex.Message);
            }
            return RedirectToAction("Read", new { id = announcementViewModelModel.ID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}