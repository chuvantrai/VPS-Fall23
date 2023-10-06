using Microsoft.EntityFrameworkCore;

namespace Service.ManagerVPS.Repositories.Interfaces
{
    public interface IVpsRepository<TEntity>
        where TEntity : class
    {
        DbSet<TEntity> Entities { get; }
        Task<TEntity> Create(TEntity entity);
        Task Delete(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Find(params object[][] keys);
        Task<int> SaveChange();
    }
}
