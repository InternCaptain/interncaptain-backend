using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.GraphQL
{
    public class Query
    {
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<User>> GetUsers([Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Users;
        }

        public async Task<User> GetCurrentUser(
            [GlobalState] string currentUser,
            [Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Users.Single(user => user.Email == currentUser);
        }

        public async Task<IEnumerable<string>> GetCurrentUserRoles(
            [Service] IHttpContextAccessor httpContext)
        {
            return from claim in httpContext.HttpContext.User.FindAll(ClaimTypes.Role) select claim.Value;
        }
    }
}