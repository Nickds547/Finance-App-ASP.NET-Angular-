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
        public async Task<ActionResult<IEnumerable<UserObjectDTO>>> GetUserObjects()
        {
            return await _context.UserObjects.Select(item => ItemToDTO(item)).ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserObjectDTO>> GetUserObject(string id)
        {
            var userObject = await _context.UserObjects.FindAsync(id);

            if (userObject == null)
            {
                return NotFound();
            }

            return ItemToDTO(userObject);
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserObject(string email, UserObjectDTO userObjectDTO)
        {
            if (email != userObjectDTO.Email)
            {
                return BadRequest();
            }

            var userObject = await _context.UserObjects.FindAsync(email);

            if(userObject == null)
            {
                return BadRequest();
            }

            userObject.Email = userObjectDTO.Email;
            userObject.Name = userObjectDTO.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserObjectExists(email))
            {
                if (!UserObjectExists(email))
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
        public async Task<ActionResult<UserObjectDTO>> PostUserObject(UserObject userObject)
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

            return CreatedAtAction("GetUserObject", new { id = userObject.Email }, ItemToDTO(userObject));
        }


        private bool UserObjectExists(string id)
        {
            return _context.UserObjects.Any(e => e.Email == id);
        }

        private static UserObjectDTO ItemToDTO(UserObject item) =>
            new UserObjectDTO
            {
                Name = item.Name,
                Email = item.Email
            };
    }
}
