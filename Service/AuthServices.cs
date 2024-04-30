
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

        public AuthServices(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<string> CreateTokenAsync(Patient patient, UserManager<Patient> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,patient.UserName),
                new Claim(ClaimTypes.Email,patient.Email)
            };

            var userRoles = await userManager.GetRolesAsync(patient);

            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateTokenAsync(Doctor doctor, UserManager<Doctor> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,doctor.UserName),
                new Claim(ClaimTypes.Email,doctor.Email)
            };

            //var userRoles = await userManager.GetRolesAsync(doctor);

            //foreach (var role in userRoles)
            //    authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateTokenAsync(Nurse nurse, UserManager<Nurse> userManager)
        {

            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,nurse.UserName),
                new Claim(ClaimTypes.Email,nurse.Email)
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateTokenAsync(Receptionist receptionist, UserManager<Receptionist> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,receptionist.UserName),
                new Claim(ClaimTypes.Email,receptionist.Email)
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public async Task<string> CreateTokenAsync(RadiologyLab radiologyLab, UserManager<RadiologyLab> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,radiologyLab.UserName),
                new Claim(ClaimTypes.Email,radiologyLab.Email)
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateTokenAsync(CheckupLab checkupLab, UserManager<CheckupLab> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,checkupLab.UserName),
                new Claim(ClaimTypes.Email,checkupLab.Email)
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateTokenAsync(Admin admin, UserManager<Admin> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,admin.UserName),
                new Claim(ClaimTypes.Email,admin.Email)
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateTokenAsync(Pharmacy pharmacy, UserManager<Pharmacy> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,pharmacy.UserName),
                new Claim(ClaimTypes.Email,pharmacy.Email)
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
