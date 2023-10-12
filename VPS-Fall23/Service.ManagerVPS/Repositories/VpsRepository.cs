using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class VpsRepository<T> : IVpsRepository<T>
        where T : class
    {
        protected DbSet<T> entities;
        protected FALL23_SWP490_G14Context context { get; set; }

        public VpsRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
        {
            context = fALL23_SWP490_G14Context;
            entities = context.Set<T>();
        }

        public DbSet<T> Entities => entities;

        public async Task<T> Create(T entity)
        {
            var addResult = await this.entities.AddAsync(entity);
            return addResult.Entity;
        }

        public async Task Delete<TKeyType>(params TKeyType[] key)
            where TKeyType : struct
        {
            T data = await this.Find(key);
            await this.Delete(data);
        }
        public Task Delete(T entity)
        {
            return Task.Run(() => this.entities.Remove(entity));
        }

        public async Task<T> Find(params object[] keys)
        {
            T entity = await this.entities.FindAsync(keys)
                       ?? throw new ClientException(ResponseNotification.NOT_FOUND);
            return entity;
        }

        public async Task<int> SaveChange()
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                int recordChanged = await context.SaveChangesAsync();
                transaction.Commit();
                return recordChanged;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public Task<T> Update(T entity)
        {
            T updatedEntity = entities.Update(entity).Entity;
            return Task.FromResult(updatedEntity);
        }
    }
}