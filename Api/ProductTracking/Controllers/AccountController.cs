using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductTracking.Authentication;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProductTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IService<UserDTO> service;
        
        public AccountController(IService<UserDTO> context)
        {
            service = context;
        }

        [HttpPost("token")]
        public IActionResult Token(LoginModel loginModel)
        {
            var identity = GetIdentity(loginModel.Email, loginModel.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid login or password." });
            }

            var now = DateTime.UtcNow;

            //create JWT-token
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                login = identity.Name,
                role = identity.Claims.FirstOrDefault(c => c.Type == identity.RoleClaimType).Value
            };

            return new ObjectResult(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var userView = service.GetAll().FirstOrDefault(u => u.Email == username && IsPasswordValid(password, u.Salt, u.PasswordHash));
            
            if(userView != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userView.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userView.Role)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;    
        }

        // hashing
        private byte[] ComputePasswordHash(string password, int salt)
        {
            byte[] saltBytes = new byte[4];
            saltBytes[0] = (byte)(salt >> 24);
            saltBytes[1] = (byte)(salt >> 16);
            saltBytes[2] = (byte)(salt >> 8);
            saltBytes[3] = (byte)(salt);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] preHashed = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, preHashed, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, preHashed, passwordBytes.Length, saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(preHashed);
        }

        // password check with correct password
        private bool IsPasswordValid(string passwordToValidate, int salt, byte[] correctPasswordHash)
        {
            byte[] hashedPassword = ComputePasswordHash(passwordToValidate, salt);

            return hashedPassword.SequenceEqual(correctPasswordHash);
        }
    }
}