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

        [HttpGet("id:int",Name ="GetById")]
        public IActionResult GetbyId(int id)
        {
            var co = _context.CelestialObjects.Find(id);
            if(co == null) return NotFound();

            co.Satellites = (List<CelestialObject>)from o in _context.CelestialObjects
                            where o.OrbitedObjectId.Equals(co.Id)
                            select o;
            return Ok(co);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var co = _context.CelestialObjects.FirstOrDefault(o => o.Name == name);
            if (co.Name != name) return NotFound();

            co.Satellites = (List<CelestialObject>)from o in _context.CelestialObjects
                                                   where o.OrbitedObjectId.Equals(co.Id)
                                                   select o;
            return Ok(co);
        }
    }
}
