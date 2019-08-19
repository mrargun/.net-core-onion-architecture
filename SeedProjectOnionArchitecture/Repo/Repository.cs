using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext context;
        protected DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }


        public IQueryable<T> GetAll()
        {
            return entities.Where(s => s.Status == 1);
        }
        public async Task<T> Get(long id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id && s.Status == 1);
        }

        public IQueryable<T> GetFiltered(Expression<Func<T, bool>> whereClause)
        {
            return entities.Where(whereClause);
        }

        public int? Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            try
            {
                SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
            return entity.Id;
        }

        public IQueryable<T> GetSPFromDatabase(string _query)
        {

            return entities.FromSql(_query);

        }

        public int Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }


            context.Entry(entity).State = EntityState.Modified;

            try
            {
                SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        public void UpdateStatus(T entity, int status)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                string _query = String.Format("call sp_update_status ({0},{1});", entity.Id, status);
                GetSPFromDatabase(_query);

            }
            catch (Exception ex)
            {
            }

        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            SaveChanges();
        }
        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }
        public async void SaveChanges()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
