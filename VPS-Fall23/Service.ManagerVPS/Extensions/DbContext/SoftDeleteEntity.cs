namespace Service.ManagerVPS.Extensions.DbContext
{
    public abstract class SoftDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }
}