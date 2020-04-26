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




        //public IGenericService<Post> Posts(string username)
       // {
           // var oid = new ObjectId(id);
            //var _collection = database.GetCollection<User>("Users").Find(u=>u.Username==username).FirstOrDefault().Posts;
            
           // return _posts ?? (_posts = new GenericService<Post>(_collection));

       // }

    }
}
