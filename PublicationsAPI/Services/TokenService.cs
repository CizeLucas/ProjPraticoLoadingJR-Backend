using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PublicationsAPI.Services {
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public readonly int expirationTimeInMinutes;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            expirationTimeInMinutes = int.Parse(_config["JWT:ExpireTimeInMinutes"]);
        }
        public string CreateToken(Users user)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Uuid)
            };

            var signingCreds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expirationTimeInMinutes),
                SigningCredentials = signingCreds,
                Issuer = _config[""],
                Audience = _config[""]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public int GetExpirationTimeInMinutes()
        {
            return expirationTimeInMinutes;
        }
    }
}