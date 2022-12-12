using JWTAuthentication.Data;
using JWTAuthentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Helpers
{
    public class JwtService
    {
        //private string secureKey = "this is a very very secure key";
        private readonly IConfiguration _config;
        private readonly IUserRepository _repo;
        public JwtService(IConfiguration config, IUserRepository repo)
        {
            _repo= repo;
            _config = config;
        }

        //public string Generate(int id)
        //{
        //    var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
        //    var credentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        //    var header = new JwtHeader(credentials);
        //    // Payload must later be updated to our needs
        //    var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1));

        //    var securityToken = new JwtSecurityToken(header, payload);
        //    return new JwtSecurityTokenHandler().WriteToken(securityToken);
        //}

        public string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

        //private User Authenticate(User user)
        //{
        //    var currentUser = _repo.GetByUsername(user.Username);
        //    if (currentUser != null)
        //    {
        //        return currentUser;
        //    };
        //    return null;
        //}
    }
}
