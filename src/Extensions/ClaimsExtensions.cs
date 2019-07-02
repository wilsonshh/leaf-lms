using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using WebApplication.Models;

namespace WebApplication.Extensions
{
    public static class ClaimsExtensions
    {
        private static string GetUserEmail(this ClaimsIdentity identity)
        {
            return identity.Claims?.FirstOrDefault(c => c.Type == "WebApplication.Models.RegisterViewModel.Email")?.Value;
        }

        public static string GetUserEmail(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity != null ? GetUserEmail(claimsIdentity) : "";
        }

        private static string GetUserNameIdentifier(this ClaimsIdentity identity)
        {
            return identity.Claims?.FirstOrDefault(c => c.Type == "WebApplication.Models.RegisterViewModel.NameIdentifier")?.Value;
        }

        public static string GetUserNameIdentifier(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity != null ? GetUserNameIdentifier(claimsIdentity) : "";
        }

        private static bool IsManager(this ClaimsIdentity identity)
        {
            return identity.Claims?.FirstOrDefault(c => c.Type == CustomIdentityUser.UserTypeClaim)?.Value == "manager";
        }

        public static bool IsManager(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity != null ? IsManager(claimsIdentity) : false;
        }
    }
}