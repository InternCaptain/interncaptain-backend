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
        [UseSelection]
        [UseFiltering]
        [UseSorting]m
        public async Task<IQueryable<Internship>> GetInternships([Service] IDbContextFactory<APIContext> factory)
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
        [UseSelection]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Application>> GetApplicationsAsync([Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Applications;
        }

        [UseSelection]
        [UseFiltering]
        public IQueryable<Profile> GetProfiles(
            [Service] IDbContextFactory<APIContext> factory)
        {
            return factory.CreateDbContext().Profiles;
        }
        
    }
}