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
        

        [HttpGet]
        public ActionResult<List<userType>> GetAll() =>
            service.GetAll();


        [HttpGet("{id}")]
        public ActionResult<userType> Get(int id)
        {
            var s = service.Get(id);

            if (s == null)
                return NotFound();

            return s;
        }

        [HttpPost] 
        public IActionResult Create(userType s)
        {
            service.Add(s);
            return CreatedAtAction(nameof(Create), new {id=s.Id}, s);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, userType s)
        {
            if (id != s.Id)
                return BadRequest();

            var existingsong = service.Get(id);
            if (existingsong is null)
                return  NotFound();

            service.Update(s);

            return NoContent();
        }

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
            // יוצרים את ה-Claims על בסיס המשתמש שהתקבל
            var claims = new List<Claim>
    {
        new Claim("username", user.Name),
        new Claim("userid", user.Id.ToString()),
        new Claim("role", user.Role),
        new Claim("type", "users")
    };

            // יוצרים את הטוקן בעזרת TokenService
            var token = TokenService.GetToken(claims);

            // מחזירים JSON עם הטוקן
            return Ok(new { token = TokenService.WriteToken(token) });
        }

    }
}