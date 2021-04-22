using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}",Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var co = _context.CelestialObjects.Find(id);
            if(co == null) return NotFound();

            co.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == co.Id).ToList();
            return Ok(co);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var co = _context.CelestialObjects.Where(o => o.Name == name);
            if (!co.Any()) return NotFound();

            foreach (var o in co)
            {
                o.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == o.Id).ToList();
            }

            return Ok(co.ToList());
        }
    }
}
