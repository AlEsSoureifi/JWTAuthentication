using System.Text.Json.Serialization;

namespace JWTAuthentication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

    }
}
