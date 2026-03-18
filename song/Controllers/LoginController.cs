using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using user.Models;
using user.interfaces;
using Token.Services;

namespace user.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly Iuser _userService;

    public LoginController(Iuser userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("[action]")]
    public ActionResult<string> Login([FromBody] userType User)
    {
        if (User == null || User.Id <= 0 || string.IsNullOrWhiteSpace(User.Name))
        {
            return BadRequest("Invalid user data.");
        }

        // Validate against stored users
        var existingUser = _userService.Get(User.Id);
        if (existingUser == null)
        {
            return Unauthorized("המשתמש עם מזהה זה לא קיים במערכת.");
        }
        if (!string.Equals(existingUser.Name, User.Name, StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized("שם המשתמש לא תואם למזהה שניתן.");
        }

        var claims = new List<Claim>
        {
            new Claim("userid", existingUser.Id.ToString()),
            new Claim("username", existingUser.Name),
            new Claim("age", existingUser.age.ToString()),
            new Claim("role", existingUser.Role ?? "user"),
            new Claim("type", "users"),
        };

           //  console.log(existingUser.Role);
             
        var token = TokenService.GetToken(claims);
        return Ok(new { token = TokenService.WriteToken(token) });

    }
}
