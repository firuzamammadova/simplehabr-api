using System;
using MongoDB.Bson;
using MongoDB.Driver;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase database;

        public UnitOfWork(IDatabaseSettings _settings)
        {
            //settings = _settings ?? throw new ArgumentNullException("settings can not be null");
            var client = new MongoClient(_settings.ConnectionString);
            database = client.GetDatabase(_settings.DatabaseName);
        }

        private IPostService _posts;
        private IAuthService _users;
        private IGenericService<Comment> _comments;
        public IAuthService Users
        {
            get
            {
                return _users ?? (_users = new AuthService(database));
            }
        }

        public IPostService Posts
        {
            get
            {
                return _posts ?? (_posts = new PostService(database));
            }

        }
        public IGenericService<Comment> Comments
        {
            get
            {
                return _comments ?? (_comments = new GenericService<Comment>(database));
            }
        }

    }
}
