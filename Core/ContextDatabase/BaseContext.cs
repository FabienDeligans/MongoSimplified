﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Models;
using MongoDB.Driver;

namespace Core.ContextDatabase
{
    /// <summary>
    /// This class allow the connection with a Mongo Database.
    /// This class allow the main operation of CRUD.
    /// </summary>
    public abstract class BaseContext : IDisposable
    {
        protected IMongoDatabase MongoDatabase;
        protected MongoClient Client { get; set; }
        public abstract string ConnectionString { get; }
        public abstract string DatabaseName { get; }

        public IMongoCollection<T> Collection<T>() where T : Entity => MongoDatabase.GetCollection<T>(typeof(T).Name);

        public void Dispose()
        {
        }

        public void DropDatabase() => Client.DropDatabase(DatabaseName);

        public void DropCollection<T>() where T : Entity => MongoDatabase.DropCollection(typeof(T).Name);

        /// <summary>
        /// Get all entities of a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> QueryCollection<T>() where T : Entity => Collection<T>().AsQueryable();

        /// <summary>
        /// Get a specific entity of a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity<T>(string id) where T : Entity
        {
            return Collection<T>().Find(v => v.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Insert an entity in a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Insert<T>(T entity) where T : Entity
        {
            Collection<T>().InsertOne(entity);
        }

        /// <summary>
        /// Insert all entities of the parameter
        /// Parameter must be an IEnumerable of the same type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void InsertAll<T>(IEnumerable<T> entities) where T : Entity
        {
            if (!entities.Any()) return;
            Collection<T>().InsertMany(entities);
        }

        /// <summary>
        /// Delete one entity selected by a predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        public void RemoveOne<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            Collection<T>().DeleteOne(predicate);
        }

        /// <summary>
        /// Delete multiple entities of the same type
        /// selected by a predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        public void RemoveAll<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            Collection<T>().DeleteMany(predicate);
        }

        /// <summary>
        /// Update a single property of an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        public void UpdateProperty<T>(T entity, string propertyName, object newValue) where T : Entity
        {
            var update = Builders<T>.Update.Set(propertyName, newValue);
            Collection<T>().UpdateOne(v => v.Id == entity.Id, update);
        }

        /// <summary>
        /// Update all the entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void UpdateEntity<T>(T entity) where T : Entity
        {
            Collection<T>().ReplaceOne(v => v.Id == entity.Id, entity);
        }
    }
}