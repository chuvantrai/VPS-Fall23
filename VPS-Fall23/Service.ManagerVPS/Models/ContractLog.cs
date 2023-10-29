namespace Service.ManagerVPS.Models
{
    public partial class ContractLog
    {
        public Guid Id { get; set; }
        public int SubId { get; set; }
        public Guid ContractId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public int TypeId { get; set; }

        public virtual Contract Contract { get; set; } = null!;
        public virtual Account CreatedByNavigation { get; set; } = null!;
        public virtual GlobalStatus Type { get; set; } = null!;
    }
}
