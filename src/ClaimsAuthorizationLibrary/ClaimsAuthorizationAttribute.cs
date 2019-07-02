using System;
using System.IdentityModel.Services;
using System.Security.Claims;
using System.ServiceModel.Channels;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApplication.ClaimsAuthorizationLibrary
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        public string claimType;
        public string claimValue;

        public ClaimsAuthorizeAttribute(string type, string value) : base()
        {
            this.claimType = type;
            this.claimValue = value;
        }

        public ClaimsAuthorizeAttribute() : base()
        {
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = actionContext?.Request?.GetUserPrincipal() as ClaimsPrincipal;

            if (user != null)
            {
                ClaimsPrincipalPermission cpp = new ClaimsPrincipalPermission(claimType, claimValue);

                try
                {
                    cpp.Demand();
                }
                catch (Exception)
                {
                    base.HandleUnauthorizedRequest(actionContext);
                }

                base.OnAuthorization(actionContext);
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var user = actionContext?.Request?.GetUserPrincipal() as ClaimsPrincipal;

            if (user == null)
                return false;

            ClaimsPrincipalPermission cpp = new ClaimsPrincipalPermission(claimType, claimValue);

            try
            {
                cpp.Demand();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}