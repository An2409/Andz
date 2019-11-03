using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Core.Contracts;
using Core.Models;

namespace Shop.DataAccess.InMemory
{
    public class BaseRepository<T> : IBaseRepository<T> where T: BaseEntity
    {
        private ObjectCache cache = MemoryCache.Default;
        private List<T> items;
        private string className;

        public BaseRepository()
        {
            //type of de lay name cua class 
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }
        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(x => x.Id == t.Id);
            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw  new Exception(className + " not found");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(x => x.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(x => x.Id == Id);
            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }

    }
}
