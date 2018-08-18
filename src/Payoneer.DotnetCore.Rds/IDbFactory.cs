using Microsoft.EntityFrameworkCore;

namespace Payoneer.DotnetCore.Rds
{
    public interface IDbFactory
    {
        DbContext GetDbContext();
    }
}
