using System.Linq;
using API.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Types;

namespace API.GraphQL
{
    public class Query
    {
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<User> GetUsers([Service] ApiContext apiContext)
        {
            return apiContext.Users;
        }
    }
}