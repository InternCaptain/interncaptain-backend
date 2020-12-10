using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.GraphQL
{
    public class Mutation
    {
        public async Task<LoginOutput> LoginAsync(
            LoginInput input,
            [Service] IDbContextFactory<APIContext> factory)
        {
            var context = factory.CreateDbContext();
            if (string.IsNullOrEmpty(input.Email))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("email"))
                        .SetMessage("The email mustn't be empty.")
                        .SetCode("EMAIL_EMPTY")
                        .Build());
            }

            if (string.IsNullOrEmpty(input.Password))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("password"))
                        .SetMessage("The password mustn't be empty.")
                        .SetCode("PASSWORD_EMPTY")
                        .Build());
            }

            User userFromDatabase = (
                from user in context.Users
                where user.Email == input.Email
                select user
            ).SingleOrDefault();

            if (userFromDatabase is null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("email"))
                        .SetMessage("The specified username is invalid.")
                        .SetCode("INVALID_EMAIL")
                        .Build());
            }

            if (!BCrypt.Net.BCrypt.Verify(input.Password, userFromDatabase.Password))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("password"))
                        .SetMessage("The specified password is invalid.")
                        .SetCode("INVALID_PASSWORD")
                        .Build());
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userFromDatabase.Email),
                new Claim(ClaimTypes.Role, "user"),
            });

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Startup.GetSharedSecret()),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return new LoginOutput
            {
                Token = tokenString,
                CurrentUser = userFromDatabase
            };
        }

        public async Task<User> CreateUser(
            User user,
            [Service] IDbContextFactory<APIContext> factory,
            [Service] ITopicEventSender eventSender)
        {
            var apiContext = factory.CreateDbContext();

            user.Id = 0;

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var inDatabase = apiContext.Users.Add(user).Entity;
            await apiContext.SaveChangesAsync();

            await eventSender.SendAsync("users", inDatabase)
                .ConfigureAwait(false);

            return inDatabase;
        }
    }

    public class LoginInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginOutput
    {
        public string Token { get; set; }
        
        public User CurrentUser { get; set; }
    }
}