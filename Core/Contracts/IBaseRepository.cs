using System.Linq;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts { 
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Commit();
        void Insert(T t);
        void Update(T t);
        T Find(string Id);
        IQueryable<T> Collection();
        void Delete(string Id);
    }
}