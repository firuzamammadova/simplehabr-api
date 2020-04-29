using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<User> _users;

        public AuthService(IMongoDatabase database)
        {

            _users = database.GetCollection<User>("Users");
        }
        public string GetUsername(ObjectId id)
        {
            return _users.Find(i => i.Id == id).FirstOrDefault().Username;
        }
        public ObjectId GetUserId(string username)
        {
            return _users.Find(u => u.Username == username).FirstOrDefault().Id;
        }
        public void AddPost(ObjectId userId,ObjectId postId)
        {
            var theuser = _users.Find(u => u.Id == userId).FirstOrDefault();
            if (theuser.Posts==null)
            {
                theuser.Posts = new List<ObjectId>();
            }
            theuser.Posts.Add(postId);
            _users.ReplaceOne(user => user.Id == userId, theuser);
           
            

        }
        public void UpdatePosts(ObjectId userId, List<ObjectId> postIds)
        {
            var theuser = _users.Find(u => u.Id == userId).FirstOrDefault();
            if (theuser.Posts == null)
            {
                theuser.Posts = new List<ObjectId>();
            }
            theuser.Posts = postIds;
            _users.ReplaceOne(user => user.Id == userId, theuser);

        }
        public void DeletePost(ObjectId userId, ObjectId postId)
        {
            var theuser = _users.Find(u => u.Id == userId).FirstOrDefault();
            if (theuser.Posts != null)
            {
                theuser.Posts.Remove(postId);
            }
            _users.ReplaceOne(user => user.Id == userId, theuser);
            


        }
        public async Task<User> Login(string userName, string password)
        {

            var user = await _users.Find(x => x.Username == userName).FirstAsync();

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;

        }



        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _users.InsertOneAsync(user);

            return user;
        }

        public async Task<bool> UserExists(string userName)
        {

            if (await _users.Find(x => x.Username == userName).CountDocumentsAsync()>0)
            {
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }

                }
                return true;
            }
        }
    }
}
