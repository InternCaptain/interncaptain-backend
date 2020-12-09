using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.GraphQL
{
    public partial class Startup
    {
        public static byte[] GetSharedSecret() => Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyz1234567890");

        private void ConfigureAuthenticationServices(IServiceCollection services)
        {
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(GetSharedSecret()),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var headers = context.HttpContext.Request.Headers;
                            if (headers.ContainsKey("Authorization"))
                            {
                                context.Token = headers["Authorization"];
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}