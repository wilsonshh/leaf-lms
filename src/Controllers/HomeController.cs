using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.Extensions;
using WebApplication.Models;

namespace WebApplication.Controllers
{
  public class HomeController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    public ActionResult DashboardV1()
    {
      DashboardViewModel dasdDashboardViewModel =
          new DashboardViewModel();
      try
      {
        ApplicationUserManager applicationUserManager =
           HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        if (User.Identity.IsManager())
          dasdDashboardViewModel.TotalRegistredUsers =
              applicationUserManager.Users.Count(u => !u.Email.Contains("manager"));

        if (User.Identity.IsManager())
        {
          dasdDashboardViewModel.TotalUserUnreadMessages =
                   User.Identity.GetPendingMessages().Count;

          int total = db.Announcements.Count();

          dasdDashboardViewModel.TotalNotSeenMessages = db.Announcements.Count(a => a.AnnouncementReads.Count == 0);
          dasdDashboardViewModel.TotalSeenMessages = total - dasdDashboardViewModel.TotalNotSeenMessages;
        }
      }
      catch (Exception ex)
      {
        ModelState.AddModelError("Error", ex.Message);
      }

      return View(dasdDashboardViewModel);
    }

    public ActionResult DashboardV2()
    {
      return View();
    }

    // POST: AnnouncementViewModel/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Compose(DashboardViewModel dashboardViewModel)
    {
      try
      {
        if (!User.Identity.IsManager())
          throw new SecurityException();

        if (ModelState.IsValid)
        {
          ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

          dashboardViewModel.Announcement.From = user.NameIdentifier;
          Announcement announcement =
              Mapper.Map<AnnouncementViewModel, Announcement>(dashboardViewModel.Announcement);

          db.Announcements.Add(announcement);
          await db.SaveChangesAsync();
          return RedirectToAction("DashboardV1");
        }
      }
      catch (Exception ex)
      {
        ModelState.AddModelError("Error", ex.Message);
      }
      return View(dashboardViewModel);
    }
  }
}