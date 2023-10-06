using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Repositories.Interfaces.Base;

namespace Service.ManagerVPS.Repositories.Base
{
    public class VpsRepository<T> : IVpsRepository<T>
        where T : class
    {
        public VpsRepository() { }
        public DbSet<T> Entities => throw new NotImplementedException();

        public Task<T> Create(T entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Find(params object[][] keys)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChange()
        {
            throw new NotImplementedException();
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
