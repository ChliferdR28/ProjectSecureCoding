using System;
using System.Linq;
using ProjectSecureCoding.Models;
using ProjectSecureCoding.Data;
using BC = BCrypt.Net.BCrypt;

namespace ProjectSecureCoding.Data
{
    public class UserData : IUser
    {
        private readonly ApplicationDbContext _db;

        public UserData(ApplicationDbContext db)
        {
            _db = db;
        }

        // Login method
        public User Login(User user)
        {
            var _user = _db.User.FirstOrDefault(u => u.Username == user.Username);

            if (_user == null)
            {
                throw new Exception("User not found");
            }

            if (!BC.Verify(user.Password, _user.Password))
            {
                throw new Exception("Incorrect password");
            }

            return _user;
        }

        // Registration method
        public User Registration(User user)
        {
            try
            {
                user.Password = BC.HashPassword(user.Password);

                _db.User.Add(user);
                _db.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Registration failed: " + ex.Message);
            }
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username is required");
            }
            else
            {
                return _db.User.FirstOrDefault(u => u.Username == username);
            }
        }

        public User UpdateUser(User user)
        {
            var existingUser = _db.User.FirstOrDefault(u => u.Username == user.Username); // Assuming you have an Id property
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            existingUser.Username = user.Username;
            existingUser.Password = BC.HashPassword(user.Password); // Hash the new password
            existingUser.Role = user.Role; // Update role if needed

            _db.SaveChanges();
            return existingUser;
        }
    }
}
