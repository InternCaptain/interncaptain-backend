using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
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
        public async Task<IQueryable<User>> GetUsersAsync([Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Users;
        }

        [Authorize]
        public async Task<User> GetCurrentUserAsync(
            [GlobalState] string currentUser,
            [Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Users.Single(user => user.Email == currentUser);
        }

        [Authorize]
        public IEnumerable<string> GetCurrentUserRoles(
            [Service] IHttpContextAccessor httpContext)
        {
            return from claim in httpContext.HttpContext.User.FindAll(ClaimTypes.Role) select claim.Value;
        }
    }
}