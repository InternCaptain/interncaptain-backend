using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace API.GraphQL
{
    public class Subscription
    {
        [SubscribeAndResolve]
        public async Task<IAsyncEnumerable<User>> NewUsers([Service] ITopicEventReceiver eventReceiver)
        {
            return await eventReceiver.SubscribeAsync<string, User>("users")
                .ConfigureAwait(false);
        }
    }
}