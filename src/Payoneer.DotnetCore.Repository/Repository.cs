using Microsoft.EntityFrameworkCore;
using Payoneer.DotnetCore.Rds;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        public Repository(IDbFactory dbFactory)
        {
            if (dbFactory == null) throw new ArgumentNullException(nameof(dbFactory));

            _dbSet = dbFactory.GetDbContext().Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll() => _dbSet;

        public async Task InsertAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var entityToDelete = await _dbSet.FindAsync(id);

            if (entityToDelete == null) throw new InvalidOperationException(nameof(entityToDelete));
            _dbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
        }
    }
}
