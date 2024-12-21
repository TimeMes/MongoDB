using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithDB
{
    public interface IRepository<T>
    {
        Task<List<T>> ReadAll();
        void SaveInDB(params T[] arrayElements);
        Task<List<T>> ReadByFilter(FilterDefinition<T> filter);
        FilterDefinition<T> FilterBuilder(Expression<Func<T, bool>> func);
        void ReplaceOne(BsonDocument element, BsonElement keyWithValue);
        void DeleteOne(BsonDocument filter);
        void DeleteMany(BsonDocument filter);
    }
}
