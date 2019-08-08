using expenses_API.Data;
using expenses_API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace expenses_API.Controllers
{
    [EnableCors("*", "*", "*")] // opens our controller to be used by 3rd party applications Angular
    [RoutePrefix("auth")] // used to shorten the route
    public class AuthenticationController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody]User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                return BadRequest("Enter your Username and Password");
            try
            {
                using (var context= new AppDbContext())
                {
                    var exists = context.Users.Any(n => n.UserName == user.UserName && n.Password == user.Password);
                    if (exists) return Ok(CreateToken(user));

                    return BadRequest("Wrong Information");
                }

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [Route("register")]
        [HttpPost]
        public IHttpActionResult Register([FromBody]User user)
        {
            try
            {
                using (var context = new AppDbContext())
                {

                    var exists = context.Users.Any(n => n.UserName == user.UserName);
                    if (exists) return BadRequest("User Already Exists");
                    context.Users.Add(user);
                    context.SaveChanges();

                    return Ok(CreateToken(user));

                    
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            return null;
        }
        private JwtPackage CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Email,user.UserName)});

            const string secretKey = "your secret key goes here";
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = (JwtSecurityToken)tokenHandler.CreateJwtSecurityToken(
                subject: claims,
                signingCredentials: signinCredentials
                );

            var tokenString = tokenHandler.WriteToken(token);

            return new JwtPackage()
            {
                UserName = user.UserName,
                Token = tokenString
            };
        }
    }
}

public class JwtPackage
{
    public string Token { get; set; }
    public string UserName { get; set; }
}
