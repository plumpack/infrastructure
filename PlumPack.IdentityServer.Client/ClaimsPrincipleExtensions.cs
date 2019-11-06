using System;
using System.Security.Claims;

namespace PlumPack.IdentityServer.Client
{
    public static class ClaimsPrincipleExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claim)
        {
            if(claim.Identity?.IsAuthenticated != true)
            {
                throw new Exception("User not authenticated.");
            }

            var id = claim.FindFirst(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                throw new Exception("No NameIdentifier claim.");
            }

            Guid value;
            if (!Guid.TryParse(id.Value, out value))
            {
                throw new Exception($"The value {value} isn't a Guid.");
            }

            return value;
        }
    }
}