using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using user.Models;
using user.Services;
using user.interfaces;
using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Token.Services;
namespace SONG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        Iuser service;

        public UserController(Iuser service){
            this.service=service;
        }
        

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public ActionResult<List<userType>> GetAll() =>
            service.GetAll();


        [HttpGet("{id}")]
        public ActionResult<userType> Get(int id)
        {
            var userId = User.FindFirst("userid")?.Value;
            var userRole = User.FindFirst("role")?.Value;
            if (userId == null)
                return Unauthorized("לא ניתן לקבוע את זהות המשתמש.");

            // משתמש רגיל יכול לצפות רק בעצמו
            if (userRole != "admin" && id.ToString() != userId)
                return Forbid("אין לך הרשאה לצפות בנתונים של משתמש אחר.");

            var s = service.Get(id);

            if (s == null)
                return NotFound();

            return s;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost] 
        public IActionResult Create(userType s)
        {
            if (string.IsNullOrWhiteSpace(s.Name))
                return BadRequest("שם המשתמש הוא שדה חובה.");

            if (string.IsNullOrWhiteSpace(s.Role))
                s.Role = "user"; // תפקיד ברירת מחדל

            service.Add(s);
            return CreatedAtAction(nameof(Create), new {id=s.Id}, s);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, userType s)
        {
            var userId = User.FindFirst("userid")?.Value;
            var userRole = User.FindFirst("role")?.Value;
            if (userId == null)
                return Unauthorized("לא ניתן לקבוע את זהות המשתמש.");

            // משתמש רגיל לא יכול לערוך משתמש אחר
            if (userRole != "admin" && id.ToString() != userId)
                return Forbid("אין לך הרשאה לערוך נתונים של משתמש אחר.");

            // משתמש רגיל לא יכול לשנות את התפקיד שלו
            if (userRole != "admin")
            {
                var existingUser = service.Get(id);
                if (existingUser != null && existingUser.Role != s.Role)
                    return Forbid("אין לך הרשאה לשנות את התפקיד שלך.");
            }

            if (id != s.Id)
                return BadRequest();

            var existingsong = service.Get(id);
            if (existingsong is null)
                return  NotFound();

            service.Update(s);

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var s = service.Get(id);
            if (s is null)
                return  NotFound();

            service.Delete(id);

            return Content(service.Count.ToString());
        }
       

        [HttpPost("Login")]
        [AllowAnonymous]
   public ActionResult<string> Login([FromBody] userType user)
        {
            // ������ �� �-Claims �� ���� ������ ������
            var claims = new List<Claim>
    {
        new Claim("username", user.Name),
        new Claim("userid", user.Id.ToString()),
        new Claim("role", user.Role),
        new Claim("type", "users")
    };

            // ������ �� ����� ����� TokenService
            var token = TokenService.GetToken(claims);

            // ������� JSON �� �����
            return Ok(new { token = TokenService.WriteToken(token) });
        }

    }
}