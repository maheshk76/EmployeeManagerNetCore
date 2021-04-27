using EmpManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManager.Repository
{
    public interface IRepository<T> where T:class
    {
        List<T> Get();
        T Get(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);

    }
    public class EntityRepository<T> : IRepository<T> where T : class
    {
       private readonly EmpDbContext _context;
        public EntityRepository(EmpDbContext context)
        {
            _context = context;
        }
        public List<T> Get()
        {
            return _context.Set<T>().ToList();
        }
        public T Get(int id)
        {
           return  _context.Set<T>().Find(id);
        }
        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            T DataInDb = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(DataInDb);
            _context.SaveChanges();
        }
    }
}
