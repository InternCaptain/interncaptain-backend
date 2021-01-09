using API.Models;
using API.Models.Enums;
using AutoMapper;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.GraphQL
{
    public class Mutation
    {
        public async Task<LoginOutput> LoginAsync(
            string email, string password,
            [Service] IDbContextFactory<APIContext> factory)
        {
            var dbContext = factory.CreateDbContext();
            if (string.IsNullOrEmpty(email))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("email"))
                        .SetMessage("Email should not be empty")
                        .SetCode("EMAIL_EMPTY")
                        .Build());
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("password"))
                        .SetMessage("Password should not be empty")
                        .SetCode("PASSWORD_EMPTY")
                        .Build());
            }

            User userFromDatabase = await (
                from user in dbContext.Users
                where user.Email == email
                select user
            ).SingleOrDefaultAsync();

            if (userFromDatabase is null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("email"))
                        .SetMessage("There is no associated account for this email")
                        .SetCode("EMAIL_DOES_NOT_EXIST")
                        .Build());
            }

            if (!BCrypt.Net.BCrypt.Verify(password, userFromDatabase.Password))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("password"))
                        .SetMessage("The password is wrong")
                        .SetCode("PASSWORD_DOES_NOT_MATCH")
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

        public async Task<User> RegisterAsync(
            UserForCreate input,
            [Service] IMapper mapper,
            [Service] IDbContextFactory<APIContext> factory,
            [Service] ITopicEventSender eventSender)
        {
            var dbContext = factory.CreateDbContext();

            if (string.IsNullOrEmpty(input.Email))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("email"))
                        .SetMessage("Email should not be empty")
                        .SetCode("EMAIL_EMPTY")
                        .Build());
            }

            if (string.IsNullOrEmpty(input.Password))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("password"))
                        .SetMessage("Password should not be empty")
                        .SetCode("PASSWORD_EMPTY")
                        .Build());
            }

            int count = await dbContext.Users.CountAsync(u => u.Email == input.Email);

            if (count > 0)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("email"))
                        .SetMessage("Email already used")
                        .SetCode("EMAIL_ALREADY_EXISTS")
                        .Build());
            }

            User user = mapper.Map<UserForCreate, User>(input);
            user.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);

            var inDatabase = (await dbContext.Users.AddAsync(user)).Entity;
            await dbContext.SaveChangesAsync();

            await eventSender.SendAsync("users", inDatabase)
                .ConfigureAwait(false);

            return inDatabase;
        }

        public async Task<Application> UpdateApplicationStatusAsync(
            long applicationId, ApplicationStatus newStatus,
            [Service] IDbContextFactory<APIContext> factory)
        {
            var dbContext = factory.CreateDbContext();

            var application = await dbContext.Applications.FirstOrDefaultAsync(application => application.Id == applicationId);

            if (application == null)
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("applicationId"))
                        .SetMessage("Application not found")
                        .SetCode("APPLICATION_NOT_FOUND")
                        .Build());

            application.Status = newStatus;

            dbContext.Update(application);
            await dbContext.SaveChangesAsync();

            return application;
        }

        public async Task<Application> AddApplication(
            long internshipId, long studentId,
            [Service] IDbContextFactory<APIContext> factory)
        {
            var dbContext = factory.CreateDbContext();

            //TODO: verify if application exists

            var internship = await dbContext.Internships.FirstOrDefaultAsync(internship => internship.Id == internshipId);
            if (internship == null)
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("internship"))
                        .SetMessage("Internship not found")
                        .SetCode("INTERNSHIP_NOT_FOUND")
                        .Build());

            var student = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == studentId);
            if (student == null)
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetPath(Path.New("student"))
                        .SetMessage("Student not found")
                        .SetCode("STUDENT_NOT_FOUND")
                        .Build());

            var application = new Application
            {
                InternshipId = internshipId,
                Internship = internship,
                StudentId = studentId,
                Student = student,
                Status = ApplicationStatus.Pending
            };

            await dbContext.Applications.AddAsync(application);
            await dbContext.SaveChangesAsync();

            return application;
        }
    }
}