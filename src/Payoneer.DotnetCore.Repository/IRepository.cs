using System.Linq;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Repository
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();
        Task InsertAsync(TEntity entity);
        Task DeleteAsync(int id);
        void Update(TEntity entity);
    }
}
