using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using user.Models;
using user.Services;
using Token.Services;

namespace user.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    public LoginController() { }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] userType User)
        {
            if (User == null || string.IsNullOrEmpty(User.Id.ToString()) || string.IsNullOrEmpty(User.Role))
            {
                return BadRequest("Invalid user data.");
            }



            var claims = new List<Claim>
            {   new Claim("userid :", User.Id.ToString()),
                new Claim("username", User.Name),  
                new Claim("age", User.age.ToString()),
                new Claim("role", User.Role ?? "user"),          
                new Claim("type", "users"),
            };

            var token = TokenService.GetToken(claims);
           // return new OkObjectResult(TokenService.WriteToken(token));
            return Ok(new { token = TokenService.WriteToken(token) });

        }
}