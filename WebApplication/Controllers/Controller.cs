using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using WebAPIModels.Models;

namespace Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        NorthwindContext db = new NorthwindContext();

        [HttpGet("{id}", Name = nameof(GetRegion))] 
        [ProducesResponseType(200, Type = typeof(Region))]
        [ProducesResponseType(404)]
        public IActionResult GetRegion(int id)
        {
            Region? reg = db.Region.ToList().Find(x => x.RegionId == id);

            if (reg == null)
                return NotFound();
            else
                return Ok(reg);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Region))]
        [ProducesResponseType(400)]
        public IActionResult CreateRegion([FromBody] Region ctg)
        {
            if (ctg == null)
                return BadRequest();
           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Region.Add(ctg);
            db.SaveChanges();
           
            return CreatedAtRoute(nameof(CreateRegion), new { id = ctg.RegionId }, ctg);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Update(int id, [FromBody] Region ctg)
        {
            if (ctg == null || ctg.RegionId != id)
            {
                return BadRequest(); 
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            Region? fctg = db.Region.ToList().Find(x => x.RegionId == id);

            if (fctg == null)
                return NotFound();

            db.Region.Remove(fctg);
            db.Region.Add(ctg);
            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRegion(int id)
        {
            Region? fctg = db.Region.ToList().Find(x => x.RegionId == id);

            if (fctg == null)
                return NotFound();

        
            db.Region.Remove(fctg);
            db.SaveChanges();

            return NoContent();
        }
    }
}