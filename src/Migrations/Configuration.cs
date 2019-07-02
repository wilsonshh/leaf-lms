using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Diagnostics;
using WebApplication.Models;

namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            try
            {
                SeedUsers(context);
            }
            catch (Exception ex)
            {
                Debugger.Log(0, "Error", ex.Message);
            };
        }

        private static void SeedUsers(ApplicationDbContext applicationContext)
        {
            //  var conText = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(applicationContext);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (applicationContext.Users.FirstOrDefault() != null)
                return;

            //create managers
            for (var i = 1; i <= 2; i++)
            {
                CreateUser(userManager, "lecturer", i);
            }

            //create managers
            for (var i = 1; i <= 5; i++)
            {
                CreateUser(userManager, "student", i);
            }
        }

        private static void CreateUser(UserManager<ApplicationUser> userManager, string userPrefix, int number)
        {
            string email = $"{userPrefix}{number}@email.com";
            string name = $"{userPrefix}{number}";

            ApplicationUser u = new ApplicationUser()
            {
                Email = email,
                UserName = email,
                NameIdentifier = name,
            };

            Console.WriteLine($"Creating  user {email}");
            var result = userManager.Create(u, "password");

            AboutResult(email, result, "created");

            if (result.Succeeded)
            {
                result = userManager.SetLockoutEnabled(u.Id, false);
                AboutResult(email, result, "unlocked");
            }
        }

        private static void AboutResult(string email, IdentityResult result, string action)
        {
            if (result == IdentityResult.Success)
                Console.WriteLine($"User {email} {action}");
            else
            {
                {
                    foreach (var error in result.Errors)
                        Console.WriteLine($"User {email} clould not be {action}. Error: {error}");
                }
            }
        }
    }
}