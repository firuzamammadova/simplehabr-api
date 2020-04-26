using System;
using System.Threading.Tasks;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public interface IAuthService
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);
        Task<bool> UserExists(string userName);
    }
}
