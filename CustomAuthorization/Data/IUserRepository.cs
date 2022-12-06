using JWTAuthentication.Models;

namespace JWTAuthentication.Data
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetByUsername(string username);
        User GetById(int id);
    }
}
