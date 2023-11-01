using EstacionamentoAPI.Models;
using System.Security.Claims;

namespace EstacionamentoAPI.Extensions
{
    public static class RoleClaimsExtention
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
            };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            return claims;
        }
    }
}
