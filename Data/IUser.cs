using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectSecureCoding.Models;

namespace ProjectSecureCoding.Data
{
    public interface IUser
    {
        User Registration(User user);
        User Login(User user);

        User GetUserByUsername(string username);

        User UpdateUser(User user);

    }
}