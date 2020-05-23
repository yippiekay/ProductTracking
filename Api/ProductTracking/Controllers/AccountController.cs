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

namespace ProductTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IService<UserDTO> db;
        
        public AccountController(IService<UserDTO> context)
        {
            db = context;
        }

        [HttpPost("/token")]
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
            var userView =db.GetAll().FirstOrDefault(u => u.Email == username && u.Password == password);
            
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
    }
}