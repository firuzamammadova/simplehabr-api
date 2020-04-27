using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public class GenericService<TDocument> : IGenericService<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public GenericService(IMongoDatabase database)
        {


            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }
        public IQueryable<TDocument> GetAll() =>
                _collection.Find(book => true).ToList().AsQueryable();

        public TDocument Get(ObjectId id)
        {
            //var objectId = new ObjectId(id);
            return _collection.Find<TDocument>(book => book.Id == id).FirstOrDefault();
        }

        public void Add(TDocument book)
        {
            _collection.InsertOne(book);

        }
        public virtual void AddMany(IEnumerable<TDocument> documents)
        {

            _collection.InsertMany(documents);
        }

        public void Edit(ObjectId id, TDocument bookIn)
        {
            //var objectId = new ObjectId(id);
            _collection.ReplaceOne(book => book.Id == id, bookIn);
        }

        public void Delete(TDocument bookIn) =>
            _collection.DeleteOne(book => book.Id == bookIn.Id);

        public void Delete(ObjectId id)
        {
            // var objectId = new ObjectId(id);
            _collection.DeleteOne(book => book.Id == id);
        }



        public IQueryable<TDocument> Find(Expression<Func<TDocument, bool>> predicate)
        {
            return _collection.Find(predicate).ToList().AsQueryable();
        }

        public void Edit(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }
    }
}
