using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SONG.Models;
using SONG.Services;
using SONG.interfaces;

namespace SONG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SingController : ControllerBase
    {
        Isong service;

        

        public SingController(Isong service){
            this.service=service;
        }
        

        [HttpGet]
        public ActionResult<List<songType>> GetAll()
        {
            var userId = User.FindFirst("userid")?.Value;
            var userRole = User.FindFirst("role")?.Value;
            if (userId == null)
                return Unauthorized("לא ניתן לקבוע את זהות המשתמש.");

            // אדמין רואה את כל השירים
            if (userRole == "admin")
            {
                return Ok(service.GetAll());
            }

            // משתמש רגיל רואה רק את השירים שלו
            var myUserId = int.Parse(userId);
            var mySongs = service.GetAll().Where(s => s.UserId == myUserId).ToList();
            return Ok(mySongs);
        }


        [HttpGet("{id}")]
        public ActionResult<songType> Get(int id)
        {
            var userId = User.FindFirst("userid")?.Value;
            var userRole = User.FindFirst("role")?.Value;
            if (userId == null)
                return Unauthorized("לא ניתן לקבוע את זהות המשתמש.");

            var s = service.Get(id);

            if (s == null)
                return NotFound();

            // משתמש רגיל יכול לצפות רק בשירים שלו
            if (userRole != "admin" && s.UserId.ToString() != userId)
                return Forbid("אין לך הרשאה לצפות בשיר של משתמש אחר.");

            return s;
        }

        [HttpPost] 
        public IActionResult Create(songType s)
        {
            var userId = User.FindFirst("userid")?.Value;
            if (userId == null)
                return Unauthorized("לא ניתן לקבוע את זהות המשתמש.");

            // משתמש רגיל יכול ליצור שירים רק לעצמו
            if (!User.IsInRole("admin"))
            {
                s.UserId = int.Parse(userId);
            }
            else
            {
                // אדמין צריך להגדיר UserId בצורה מפורשת
                if (s.UserId <= 0)
                {
                    s.UserId = int.Parse(userId);
                }
            }

            if (string.IsNullOrWhiteSpace(s.Name) || string.IsNullOrWhiteSpace(s.singer))
                return BadRequest("שם השיר ואמן הם שדות חובה.");

            service.Add(s);
            return CreatedAtAction(nameof(Create), new {id=s.Id}, s);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, songType s)
        {
            var userId = User.FindFirst("userid")?.Value;
            var userRole = User.FindFirst("role")?.Value;
            if (userId == null)
                return Unauthorized("לא ניתן לקבוע את זהות המשתמש.");

            var existingsong = service.Get(id);
            if (existingsong is null)
                return  NotFound();

            // משתמש רגיל יכול לערוך רק שירים שלו
            if (userRole != "admin" && existingsong.UserId.ToString() != userId)
                return Forbid("אין לך הרשאה לערוך שיר של משתמש אחר.");

            if (id != s.Id)
                return BadRequest();

            service.Update(s);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirst("userid")?.Value;
            var userRole = User.FindFirst("role")?.Value;
            if (userId == null)
                return Unauthorized("לא ניתן לקבוע את זהות המשתמש.");

            var s = service.Get(id);
            if (s is null)
                return  NotFound();

            // משתמש רגיל יכול למחוק רק שירים שלו
            if (userRole != "admin" && s.UserId.ToString() != userId)
                return Forbid("אין לך הרשאה למחוק שיר של משתמש אחר.");

            service.Delete(id);

            return Content(service.Count.ToString());
        }
    }
}