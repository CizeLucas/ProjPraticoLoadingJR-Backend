using System.Security.Claims;

namespace PublicationsAPI.Extensions
{
    public static class Extensions
    {
            public static string? GetUuid(this ClaimsPrincipal user)
            {
                return user.Claims.SingleOrDefault(x => x.Type.Equals("uuid"))?.Value;
            }

            public static string? GetUsername(this ClaimsPrincipal user)
            {
                return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"))?.Value;
            }

            public static string? GetEmail(this ClaimsPrincipal user)
            {
                return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Email"))?.Value;
            }
    }
}