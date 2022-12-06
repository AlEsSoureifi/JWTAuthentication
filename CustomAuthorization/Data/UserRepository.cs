using JWTAuthentication.Models;
using System.Linq;

namespace JWTAuthentication.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User GetByEmail(string email)
        {
            return
            _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u =>u.Username == username);
        }

        public User GetById(int id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }
    }
}
