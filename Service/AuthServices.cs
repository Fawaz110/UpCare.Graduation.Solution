
using Core.Services.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _configuration;

        public AuthServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(Patient patient, UserManager<Patient> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,patient.UserName),
                new Claim(ClaimTypes.Email,patient.Email),
            };

            var userRoles = await userManager.GetRolesAsync(patient);
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims:authClaims,
                signingCredentials:new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature)

                
                );

           return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
