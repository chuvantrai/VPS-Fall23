namespace Service.ManagerVPS.DTO.Output
{
    public class ParkingZoneItemOutput
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        
        public string? Owner { get; set; }
        
        public DateTime? Created { get; set; }

        public bool? Status { get; set; }
    }
}
