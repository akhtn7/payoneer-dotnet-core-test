using Microsoft.EntityFrameworkCore;
using System;

namespace Payoneer.DotnetCore.Rds
{
    public class DbFactory : IDbFactory, IDisposable
    {
        private readonly DbContext _dbContext;
        public DbContext GetDbContext() => _dbContext;

        public DbFactory(ApplicationContext context) =>
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));

        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing) _dbContext?.Dispose();

            _isDisposed = true;
        }
    }
}
