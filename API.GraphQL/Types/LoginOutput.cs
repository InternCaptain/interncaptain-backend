using API.Models;

namespace API.GraphQL
{
    public class LoginOutput
    {
        public string Token { get; set; }
        
        public User CurrentUser { get; set; }
    }
}