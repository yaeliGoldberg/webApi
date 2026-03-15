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
using SONG.Hubs;
using Microsoft.AspNetCore.SignalR;
using SONG;

namespace SONG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class userController : ControllerBase
    {
        Iuser service;
        IHubContext<UserHub> hub;

        public userController(Iuser service, IHubContext<UserHub> hub)
        {
            this.service=service;
            this.hub = hub;
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
            hub.Clients.All.SendAsync("UsersChanged");
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

            hub.Clients.All.SendAsync("UsersChanged");


            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var s = service.Get(id);
            if (s is null)
                return  NotFound();

            service.Delete(id);
            hub.Clients.All.SendAsync("UsersChanged");

            return Content(service.Count.ToString());
        }

         [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] userType User)
        {
            var claims = new List<Claim>
            {
                new Claim("username", User.Name),
                new Claim("userid :", User.Id.ToString()),
                new Claim("type", "users"),
            };

            var token = TokenService.GetToken(claims);

           // return new OkObjectResult(TokenService.WriteToken(token));
            return Ok(new { token = TokenService.WriteToken(token) });

        }
    }
}