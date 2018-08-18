using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Repository
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
