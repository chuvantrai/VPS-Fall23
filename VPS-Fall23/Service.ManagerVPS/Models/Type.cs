namespace Service.ManagerVPS.Models
{
    public partial class Type
    {
        public Type()
        {
            Accounts = new HashSet<Account>();
            GlobalStatuses = new HashSet<GlobalStatus>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<GlobalStatus> GlobalStatuses { get; set; }
    }
}
