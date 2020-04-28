using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public interface IAuthService
    {
        ObjectId GetUserId(string username);
        void AddPost(ObjectId userId, ObjectId postId);
        void DeletePost(ObjectId userId, ObjectId postId);
        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);
        Task<bool> UserExists(string userName);
    }
}
