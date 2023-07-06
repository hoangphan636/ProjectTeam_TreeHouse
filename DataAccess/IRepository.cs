using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepository<T>
    {
        public T Add(T _object);
        public int Delete(T _object);
        public int Update(int id, T _object);
        public IEnumerable<T> List();
        public T Find(int Id);
        public bool Exists(int id);
        public int SaveChanges();
    }
}
