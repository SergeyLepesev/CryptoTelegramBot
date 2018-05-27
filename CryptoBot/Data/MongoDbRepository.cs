using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace CryptoBot.Data
{
    public class MongoDbRepository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _db;

        public MongoDbRepository(DataContext dataContext)
        {
            _db = dataContext;
        }

        public IEnumerable<T> GetItems(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
                return _db.GetItems<T>().Find(Builders<T>.Filter.Empty).ToList();

            return _db.GetItems<T>().Find(filter).ToList();
        }

        public void Create(T item)
        {
            _db.GetItems<T>().InsertOne(item);
        }
       

        public void Delete(Expression<Func<T, bool>> filter)
        {
            _db.GetItems<T>().FindOneAndDelete(filter);
        }

        public void DeleteMany(Expression<Func<T, bool>> filter)
        {
            _db.GetItems<T>().DeleteMany(filter);
        }

        public T GetItem(Expression<Func<T, bool>> filter)
        {
            return _db.GetItems<T>().Find(filter).FirstOrDefault();
        }

        public void Update(T item, Expression<Func<T, bool>> filter)
        {
            _db.GetItems<T>().ReplaceOne(filter, item);
        }
    }
}