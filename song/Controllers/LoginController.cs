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
    private readonly userService _userService;

    public LoginController(userService userService)
    {
        _userService = userService;
    }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] userType User)
        {
            if (User == null || string.IsNullOrEmpty(User.Id.ToString()))
            {
                return BadRequest("Invalid user data.");
            }

            // Check if user exists in the users list
            var existingUser = _userService.Users.FirstOrDefault(u => u.Id == User.Id);
            if (existingUser == null)
            {
                return BadRequest("User does not exist in the system.");
            }

            // Use the actual user data from the system
            User = existingUser;

            var claims = new List<Claim>
            {   new Claim("userid:", User.Id.ToString()),
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