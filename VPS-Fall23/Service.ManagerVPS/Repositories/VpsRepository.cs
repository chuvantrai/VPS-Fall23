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

        public VpsRepository(FALL23_SWP490_G14Context fall23Swp490G14Context)
        {
            context = fall23Swp490G14Context;
            entities = context.Set<T>();
        }

        public DbSet<T> Entities => entities;

        public virtual async Task<T> Create(T entity)
        {
            var addResult = await this.entities.AddAsync(entity);
            return addResult.Entity;
        }

        public virtual Task CreateRange(List<T> listEntity)
        {
            return Task.Run(() => entities.AddRange(listEntity));
        }

        public async Task Delete<TKeyType>(params TKeyType[] key)
            where TKeyType : struct
        {
            T data = await this.Find(key);
            await this.Delete(data);
        }

        public virtual Task Delete(T entity)
        {
            return Task.Run(() => this.entities.Remove(entity));
        }

        public virtual Task DeleteRange(List<T> listEntity)
        {
            return Task.Run(() => entities.RemoveRange(listEntity));
        }

        public virtual async Task<T> Find(params object[] keys)
        {
            T entity = await this.entities.FindAsync(keys)
                       ?? throw new ClientException(ResponseNotification.NOT_FOUND);
            return entity;
        }

        public  Task<IEnumerable<PromoCode>> GetListPromoCodeByListId(IEnumerable<Guid> listPromoCodeId)
        {
            return Task.FromResult<IEnumerable<PromoCode>>(
                context.PromoCodes
                    .Include(x => x.ParkingZone)
                    .Include(x => x.PromoCodeInformation)
                    .Where(x => listPromoCodeId.Contains(x.Id))
            );
        }

        public async Task<IEnumerable<PromoCode>?> GetListPromoCodeNeedSendCode()
        {
            var listPromoCode = await context.PromoCodes
                .Include(x => x.ParkingZone)
                .Include(x => x.PromoCodeInformation)
                .Where(x => x.UserReceivedCode == false || x.UserReceivedCode == null).ToListAsync();

            if (listPromoCode.Count == 0) return null;
            foreach (var promoCode in listPromoCode)
            {
                promoCode.UserReceivedCode = true;
            }

            await context.SaveChangesAsync();
            return listPromoCode.AsEnumerable();
        }

        public virtual async Task<int> SaveChange()
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

        public virtual Task<T> Update(T entity)
        {
            T updatedEntity = entities.Update(entity).Entity;
            return Task.FromResult(updatedEntity);
        }
    }
}