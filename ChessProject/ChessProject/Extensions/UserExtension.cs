using ChessProject.DL.Enums;
using System.Security.Claims;

namespace ChessProject.Extensions
{
    public static class UserExtension
    {
        public static bool IsConnected(this ClaimsPrincipal claims)
        {
            return claims.Identity!.IsAuthenticated;
        }

        public static Role GetRole(this ClaimsPrincipal claims)
        {
            return Enum.Parse<Role>(claims.FindFirst(ClaimTypes.Role)!.Value);
        }
    }
}
