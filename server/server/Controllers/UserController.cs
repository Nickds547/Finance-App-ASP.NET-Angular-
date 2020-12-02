using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.JWT;
using server.Models;
using server.Services;
using System.Text.Json.Serialization;
using server.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;

namespace server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserObjectContext _context;
        private UserServices _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public UserController(UserObjectContext context, IJwtAuthManager jwtAuthManager)
        {
            _context = context;
            _userService = new UserServices(context);
            _jwtAuthManager = jwtAuthManager;
        }

        // GET: api/User
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserObjectDTO>>> GetUserObjects()
        {
            return await _context.UserObjects.Select(item => ItemToDTO(item)).ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserObjectDTO>> GetUserObject(string id)
        {
            var userObject = await _userService.getUserById(int.Parse(id));

            //int tokenId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", "").Replace(" ", "");

            //System.Diagnostics.Debug.WriteLine("Access Token" + accessToken);

            int tokenId = _jwtAuthManager.GetUserIdFromJwtToken(accessToken);


            //ensuring the user can only view their own account
            if (tokenId != userObject.Id)
            {
                return BadRequest();
            }


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
            if(int.Parse(id) != userObjectDTO.Id)
            {
                return BadRequest();
            }

            if(await _userService.getUserByEmail(userObjectDTO.Email) != null) //Checking if a user with the new email address already exists
            {
                return Conflict();
            }

            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", "").Replace(" ", "");
            int tokenId = _jwtAuthManager.GetUserIdFromJwtToken(accessToken);

            //ensuring the user who sent the request is updating their own account
            if(tokenId != userObjectDTO.Id || tokenId != int.Parse(id))
            {
                return BadRequest();
            }

            var user = await _userService.UpdateUser(id, userObjectDTO);

            if (user == null)
            {
                return BadRequest();
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

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
            };

            var jwtResult = _jwtAuthManager.GenerateToken(user.Email, claims, DateTime.Now);
            
            return Ok(new LoginResult
                {
                    User = ItemToDTO(user),
                    AccessToken = jwtResult.AccessToken,
                    //RefreshToken = jwtResult.RefreshToken.TokenString
                }    
            );
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserObjectDTO>> PostUserObject(UserObject userObject)
        {
            userObject.Role = Roles.User;
            var user = await _userService.AddUser(userObject);

            if(user == null)
            {
                return Conflict();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id+""),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var jwtResult = _jwtAuthManager.GenerateToken(user.Email, claims, DateTime.Now);

            return Ok(new LoginResult
            {
                User = ItemToDTO(user),
                AccessToken = jwtResult.AccessToken,
                //RefreshToken = jwtResult.RefreshToken.TokenString
            }
            );
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

        public class LoginResult
        {
            [JsonPropertyName("user")]
            public UserObjectDTO User { get; set; }
            [JsonPropertyName("accessToken")]
            public string AccessToken { get; set; }
            [JsonPropertyName("refreshToken")]
            public string RefreshToken { get; set; }
        }
    }
}
