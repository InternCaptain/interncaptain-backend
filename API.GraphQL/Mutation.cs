using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace API.GraphQL
{
    public class Mutation
    {

        public async Task<string> Login(string email, string password, [Service] HttpContext httpContext)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email)
            };

            var identity = new ClaimsIdentity(claims);

            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(principal);

            return principal.Identities.First().Name;
        }
    }
}