using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logger.Service.Interfaces;

namespace Logger.Service
{
    public class ServiceBase<T> : IServiceBase<T> where T : class
    {
        public DbContext Context { get; set; }

        public ServiceBase( DbContext context)
        {
            Context = context;
        }

        public IDbSet<T> DbSet
        {
            get { return Context.Set<T>(); }
        }

        public virtual async Task<T> Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Cannot add NULL entity");
            }

            var result = DbSet.Add(entity);
            await Context.SaveChangesAsync();
            return result;
        }

        public virtual async Task<int> Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Cannot update NULL entity");
            }
            Context.Entry(entity).State = EntityState.Modified;
            return await Context.SaveChangesAsync();
        }

        public virtual async Task<int> Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Cannot delete NULL entity");
            }
            DbSet.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public virtual async Task<IList<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<IList<T>> Get(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }
    }
}