﻿namespace Labinator2016R1.Tests.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Labinator2016R1.Lib.Models;
    using Lib.Headers;
    class FakeDatabase : ILabinatorDb
    {
        public IQueryable<T> Query<T>() where T : class
        {
            return Sets[typeof(T)] as IQueryable<T>;
        }
        public void Dispose() { }

        public void AddSet<T>(IQueryable<T> objects)
        {
            Sets.Add(typeof(T), objects);
        }

        public void Add<T>(T entity) where T : class
        {
            //if (typeof(T) == typeof(Log))
            //{
            //    LogAdded.Add(entity);
            //}
            //else
            //{
                Added.Add(entity);
            //}
        }
        public void Update<T>(T entity) where T : class
        {
            //if (typeof(T) == typeof(Log))
            //{
            //    LogUpdated.Add(entity);
            //}
            //else
            //{
                Updated.Add(entity);
            //}
        }
        public void Remove<T>(T entity) where T : class
        {
            //if (typeof(T) == typeof(Log))
            //{
            //    LogRemoved.Add(entity);
            //}
            //else
            //{
                Removed.Add(entity);
            //}
        }
        public void SaveChanges()
        {
            saved++;
        }
        public Dictionary<Type, object> Sets = new Dictionary<Type, object>();
        public List<object> Added = new List<object>();
        public List<object> LogAdded = new List<object>();
        public List<object> Updated = new List<object>();
        public List<object> LogUpdated = new List<object>();
        public List<object> Removed = new List<object>();
        public List<object> LogRemoved = new List<object>();
        public int saved = 0;

    }
}