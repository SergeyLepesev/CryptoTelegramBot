using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CryptoBot.Data
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetItems(Expression<Func<T, bool>> filter = null);
        T GetItem(Expression<Func<T, bool>> filter);
        void Create(T item);
        void Update(T item, Expression<Func<T, bool>> filter);
        void Delete(Expression<Func<T, bool>> filter);
        void DeleteMany(Expression<Func<T, bool>> filter);
    }
}