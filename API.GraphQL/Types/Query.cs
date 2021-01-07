using System.Linq;
using System.Threading.Tasks;
using API.Models;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
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

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Internship>> GetInternshipsAsync([Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Internships;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Company>> GetCompaniesAsync([Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Companies;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Application>> GetApplicationsAsync([Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Applications;
        }
    }
}