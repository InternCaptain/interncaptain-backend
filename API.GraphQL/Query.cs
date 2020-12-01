using System.Linq;
using System.Threading.Tasks;
using API.Models;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace API.GraphQL
{
    public class Query
    {
        
        private ApiContext _apiContext { get; }

        public Query(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<User>> GetUsers()
        {
            return _apiContext.Users;
        }
    }
}