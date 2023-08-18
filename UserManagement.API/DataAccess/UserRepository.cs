using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.DataAccess.Entities;

namespace UserManagement.API.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManagementContext _dbContext;
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManagementContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return _dbContext.Users.Include(u => u.Address).Include(u => u.Company).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User GetUserById(string id)
        {
            try
            {
                return _dbContext.Users.Include(u => u.Address).Include(u => u.Company).FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
