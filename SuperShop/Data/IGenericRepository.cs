using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IGenericRepository<T > where T : class
    {
        //IQueryable<T> GetAll();

        //ajuste para possibilitar a ordenação
        IQueryable<T> GetAll(Expression<Func<T, object>>? orderBy=null, bool ascending = true);

        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);

    }
}
