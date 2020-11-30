using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace server.Services
{

    public class UserServices
    {
        private readonly UserObjectContext _context;
        public UserServices(UserObjectContext context)
        {
            _context = context;
        }

        public Task<string> getUserRole(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isAnExistingUser(int id)
        {
            var userObject = await  getUserById(id);

            if(userObject == null)
            {
                return false;
            }

            return true;
        }

        public Task<bool> isValidUserCredentials(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<UserObject> getUserById(int id)
        {
            var userObject = await _context.UserObjects.FindAsync(id);

            return userObject;
        }

        public async Task<UserObject> getUserByEmail(string email)
        {
            UserObject user = _context.UserObjects.Where(a => a.Email == email).SingleOrDefault();

            return user;
        }

        public async Task<bool> UserObjectExists(int id)
        {
            return  _context.UserObjects.Any(e => e.Id == id);
        }


        public async Task<UserObject> AddUser(UserObject userObject)
        {


            userObject.Password = BC.HashPassword(userObject.Password);

            _context.UserObjects.Add(userObject);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await getUserByEmail(userObject.Email) != null)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return userObject;
        }

        public async Task<UserObject> Login(UserObject userObject)
        {
            var user = await getUserById(userObject.Id);

            if (user.Email != userObject.Email || user == null)
            {
                return null;
            }

            if (userObject == null || !BC.Verify(userObject.Password, user.Password))
            {
                return null;
            }

            return user;
        }

    }
}
