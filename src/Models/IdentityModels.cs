using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CustomIdentityUser : IdentityUser
    {
        public static string UserTypeClaim = "WebApplication.Models.RegisterViewModel.UserType";
        public string NameIdentifier { get; set; }
    }

    public class ApplicationUser : CustomIdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            userIdentity.AddClaim(new Claim("WebApplication.Models.RegisterViewModel.NameIdentifier", NameIdentifier));
            userIdentity.AddClaim(new Claim("WebApplication.Models.RegisterViewModel.Email", Email));

            if (Email.Contains("@") && string.Compare(Email.Split('@')[0], "lecturer1", true) == 0)
                userIdentity.AddClaim(new Claim(UserTypeClaim, "manager"));

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<AnnouncementRead> AnnouncementsRead { get; set; }

        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Get users that didn't read an announcement.
        /// </summary>
        /// <remarks>
        /// For performance only, is executing a direct sql with not in clause
        /// Returning AnnoucnemtRead object because Users does not permit SqlQuery
        /// </remarks>
        /// <returns>A collection of <c>AnnouncementRead</c> with users that didn't read the specified announcement</returns>

        public IEnumerable<AnnouncementRead> GetUsersNotinAnnouncemntRead(Announcement annoucement)
        {
            return AnnouncementsRead.SqlQuery(
                 $"SELECT convert(int, 0) as ID, [NameIdentifier] as UserName,[Email] as UserEmail, GetDate() as DateTimeRead, CONVERT(bit ,0) as Status FROM[dbo].[AspNetUsers] where[Email] not in (select UserEmail from[dbo].[AnnouncementReads] where Announcement_ID = {annoucement.ID})");
        }

        internal object DbSet<T>()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Alvaro Pereira EL pattern

            modelBuilder.Entity<AnnouncementReadViewModel>()
                  .Ignore(c => c.Status);

            //set some Global Standards - prevent acidental Cascade Delete
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //By Default DateTime will be mappend to sql/server datetime2 instead date/lvaro
            modelBuilder.Properties<DateTime>()
               .Configure(p => p.HasColumnType("datetime2"));

            #endregion Alvaro Pereira EL pattern
        }
    }
}