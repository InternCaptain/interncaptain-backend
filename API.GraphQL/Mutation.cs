using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using HotChocolate;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace API.GraphQL
{
    public class Mutation
    {
        private ApiContext _apiContext { get; }
        private ITopicEventSender _eventSender { get; set; }


        public Mutation(ApiContext apiContext, ITopicEventSender eventSender)
        {
            _apiContext = apiContext;
            _eventSender = eventSender;
        }

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

        public async Task<User> CreateUser(User user)
        {
            var inDatabase = _apiContext.Users.Add(user).Entity;
            _apiContext.SaveChanges();

            await _eventSender.SendAsync("users", inDatabase)
                .ConfigureAwait(false);

            return inDatabase;
        }
    }
}