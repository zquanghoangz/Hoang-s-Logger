using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Logger.Service.Interfaces
{
    public interface IServiceBase<T> where T : class
    {
        Task<IList<T>> GetAll();
        Task<IList<T>> Get(Expression<Func<T, bool>> predicate);
        Task<T> Add(T entity);
        Task<int> Delete(T entity);
        Task<int> Update(T entity);
    }
}