namespace Service.ManagerVPS.Models
{
    public partial class Account
    {
        public Account()
        {
            Cities = new HashSet<City>();
            Communes = new HashSet<Commune>();
            ContractLogs = new HashSet<ContractLog>();
            Districts = new HashSet<District>();
            ParkingTransactionCheckinByNavigations = new HashSet<ParkingTransaction>();
            ParkingTransactionCheckoutByNavigations = new HashSet<ParkingTransaction>();
            Reports = new HashSet<Report>();
        }

        public int TypeId { get; set; }
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public Guid? CommuneId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsBlock { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Avatar { get; set; }
        public int? VerifyCode { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? ExpireVerifyCode { get; set; }

        public virtual Commune? Commune { get; set; }
        public virtual Type Type { get; set; } = null!;
        public virtual ParkingZoneAttendant? ParkingZoneAttendant { get; set; }
        public virtual ParkingZoneOwner? ParkingZoneOwner { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<Commune> Communes { get; set; }
        public virtual ICollection<ContractLog> ContractLogs { get; set; }
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<ParkingTransaction> ParkingTransactionCheckinByNavigations { get; set; }
        public virtual ICollection<ParkingTransaction> ParkingTransactionCheckoutByNavigations { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
