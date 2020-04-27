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

        private IGenericService<Post> _posts;
        private IAuthService _users;

        public IAuthService Users
        {
            get
            {
                return _users ?? (_users = new AuthService(database));
            }
        }

        public IGenericService<Post> Posts
        {
            get
            {
                return _posts ?? (_posts = new GenericService<Post>(database));
            }

        }

    }
}
