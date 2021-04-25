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

		[HttpGet("{id:int}", Name = "GetById")]
		public IActionResult GetById(int id)
		{
			var co = _context.CelestialObjects.Find(id);
			if (co == null) return NotFound();

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

		[HttpGet]
		public IActionResult GetAll()
		{
			var celestialObjects = _context.CelestialObjects.ToList();
			foreach (var o in celestialObjects)
			{
				o.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == o.Id).ToList();
			}
			return Ok(celestialObjects);
		}

		[HttpPost]
		public IActionResult Create([FromBody] CelestialObject celestialObject)
		{
			_context.CelestialObjects.Add(celestialObject);
			_context.SaveChanges();
			return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, CelestialObject celestialObject)
        {
			var oldObject = _context.CelestialObjects.Find(id);
			if (oldObject == null) return NotFound();

			oldObject.Name = celestialObject.Name;
			oldObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
			oldObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

			_context.CelestialObjects.Update(oldObject);
			_context.SaveChanges();

			return NoContent();
        }

		[HttpPatch("{id}/{name}")]
		public IActionResult RenameObject(int id, string name)
        {
			var updatingObject = _context.CelestialObjects.Find(id);
			if (updatingObject == null) return NotFound();

			updatingObject.Name = name;
			_context.CelestialObjects.Update(updatingObject);
			_context.SaveChanges();

			return NoContent();
        }

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
        {
			var objects = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id);
			if (objects.Count()==0) return NotFound();

			_context.CelestialObjects.RemoveRange(objects);
			_context.SaveChanges();

			return NoContent();
        }
	}
}
