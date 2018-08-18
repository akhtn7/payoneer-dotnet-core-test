using Microsoft.EntityFrameworkCore;
using Payoneer.DotnetCore.Rds;
using System;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(IDbFactory dbFactory)
        {
            if (dbFactory == null) throw new ArgumentNullException(nameof(dbFactory));

            _context = dbFactory.GetDbContext();
        }

        public Task CommitAsync() => _context.SaveChangesAsync();
    }
}
