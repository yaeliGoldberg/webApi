using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SONG.Models;
using SONG.Services;
using SONG.interfaces;

namespace SONG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SingController : ControllerBase
    {
        Isong service;

        public SingController(Isong service){
            this.service=service;
        }
        

        [HttpGet]
        public ActionResult<List<songType>> GetAll() =>
            service.GetAll();


        [HttpGet("{id}")]
        public ActionResult<songType> Get(int id)
        {
            var s = service.Get(id);

            if (s == null)
                return NotFound();

            return s;
        }

        [HttpPost] 
        public IActionResult Create(songType s)
        {
            service.Add(s);
            return CreatedAtAction(nameof(Create), new {id=s.Id}, s);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, songType s)
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
    }
}