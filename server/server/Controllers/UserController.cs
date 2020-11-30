using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.JWT;
using server.Models;
using server.Services;
using BC = BCrypt.Net.BCrypt;

namespace server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserObjectContext _context;
        private UserServices _userService;
        private readonly IJwtAuthManager jwtAuthManager;

        public UserController(UserObjectContext context)
        {
            _context = context;
            _userService = new UserServices(context);
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
        public async Task<IActionResult> PutUserObject(string id, UserObjectDTO userObjectDTO)
        {
            int _id;
            try //attempting to convert id into an int
            {
                _id = int.Parse(id);
            }
            catch (Exception e)
            {
                return BadRequest();
            };

            if (_id != userObjectDTO.Id) //ensuring the id sent and the id of the userObjectDTO are the same
            {
                return BadRequest();
            }

            if(await _userService.isAnExistingUser(_id) == false) //ensuring the user exists in the DB
            {
                return BadRequest();
            }

            var userObject = await _userService.getUserById(_id); 

            if(await _userService.getUserByEmail(userObjectDTO.Email) != null) //Checking if a user with the new email address already exists
            {
                return Conflict();
            }

            userObject.Email = userObjectDTO.Email; //changing the userObject from the DB
            userObject.Name = userObjectDTO.Name;


            try //attempting to save changes
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await _userService.UserObjectExists(_id))
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
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserObjectDTO>> LoginUserObject(UserObject userObject)
        {
            var user = await _userService.Login(userObject);

            if(user == null)
            {
                return BadRequest();
            }

            return ItemToDTO(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserObjectDTO>> PostUserObject(UserObject userObject)
        {
            var user = await _userService.AddUser(userObject);

            if(user == null)
            {
                return Conflict();
            }

            return CreatedAtAction("AddUserObject", new { id = user.Id}, ItemToDTO(userObject));
        }


        //Private method to convert a UserObject into a Data Transfer Object so sensitive data isnt passed around
        private static UserObjectDTO ItemToDTO(UserObject item) =>
            new UserObjectDTO
            {
                Name = item.Name,
                Email = item.Email,
                Id = item.Id,
                Role = item.Role
            };
    }
}
