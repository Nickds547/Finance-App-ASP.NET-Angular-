using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserObjectContext _context;

        public UserController(UserObjectContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserObject>>> GetUserObjects()
        {
            return await _context.UserObjects.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserObject>> GetUserObject(string id)
        {
            var userObject = await _context.UserObjects.FindAsync(id);

            if (userObject == null)
            {
                return NotFound();
            }

            return userObject;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserObject(string id, UserObject userObject)
        {
            if (id != userObject.Email)
            {
                return BadRequest();
            }

            _context.Entry(userObject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserObjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserObject>> PostUserObject(UserObject userObject)
        {
            _context.UserObjects.Add(userObject);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserObjectExists(userObject.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserObject", new { id = userObject.Email }, userObject);
        }


        private bool UserObjectExists(string id)
        {
            return _context.UserObjects.Any(e => e.Email == id);
        }
    }
}
