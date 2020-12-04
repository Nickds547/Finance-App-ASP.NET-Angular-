using Microsoft.AspNetCore.Mvc;
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
            UserObject user = await _context.UserObjects.Where(a => a.Email == email).SingleOrDefaultAsync();

            return user;
        }

        public bool UserObjectExists(int id)
        {
            return  _context.UserObjects.Any(e => e.Id == id);
        }


        public async Task<UserObject> AddUser(UserObject userObject)
        {
            var userExists = await getUserByEmail(userObject.Email);

            if (userExists != null)
            {
                return null;
            }

            userObject.Password = BC.HashPassword(userObject.Password);

            _context.UserObjects.Add(userObject);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) when (getUserByEmail(userObject.Email) != null)
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

        public async Task<(UserObject, string)> Login(UserObject userObject)
        {
            var user = await getUserByEmail(userObject.Email);

            if (user == null)
            {
                return (null, "User Not Found");
            }

            if (userObject == null || !BC.Verify(userObject.Password, user.Password))
            {
                return (null, "Invalid Credentials"); ;
            }

            return (user, "Login Successful");
        }


        public async Task<UserObject> UpdateUser(string id, UserObjectDTO userObjectDTO)
        {
            int _id;
            try //attempting to convert id into an int
            {
                _id = int.Parse(id);
            }
            catch (Exception e)
            {
                return null;
            };

            if (_id != userObjectDTO.Id) //ensuring the id sent and the id of the userObjectDTO are the same
            {
                return null;
            }

            if (await isAnExistingUser(_id) == false) //ensuring the user exists in the DB
            {
                return null;
            }

            var userObject = await getUserById(_id);

            userObject.Email = userObjectDTO.Email; //changing the userObject from the DB
            userObject.Name = userObjectDTO.Name;


            try //attempting to save changes
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserObjectExists(_id))
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


    }
}
