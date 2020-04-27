using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public interface IAuthService
    {
        ObjectId GetUserId(string username);
        User AddPost(ObjectId userId, ObjectId postId);
        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);
        Task<bool> UserExists(string userName);
    }
}
