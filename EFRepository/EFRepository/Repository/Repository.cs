using EFRepository.Contracts;
using EFRepository.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFRepository.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext context;

        private readonly DbSet<T> entities;

        public Repository(DbContext context)
        {
            this.context = context;
            this.entities = this.context.Set<T>();
        }

        public T Get(long id)
        {
            return this.entities.Find(id);
        }

        public async Task<T> GetAsync(long id)
        {
            return await this.entities.FindAsync(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.entities.FirstOrDefault(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.entities.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return this.entities;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return this.entities.Where(predicate);
        }

        public IQueryable<T> GetPerPage(int perPage, int pageNumber)
        {
            return this.entities.Skip((pageNumber - 1) * perPage).Take(perPage);
        }

        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            this.entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            this.entities.RemoveRange(entity);
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public void AddOrUpdate(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            if (this.context.Entry(entity).State == EntityState.Detached)
            {
                this.entities.Add(entity);
            }
            else
            {
                this.context.Entry(entity).State = EntityState.Modified;
            }

            this.context.SaveChanges();
        }
    }
}
