using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        Task<T> Get(long id);
        IQueryable<T> GetFiltered(Expression<Func<T, bool>> whereClause);
        int? Insert(T entity);
        void UpdateStatus(T entity, int status);
        int Update(T entity);
        IQueryable<T> GetSPFromDatabase(string _query);
        void Delete(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
