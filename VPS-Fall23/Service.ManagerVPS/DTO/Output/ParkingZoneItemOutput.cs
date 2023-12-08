using NetTopologySuite.Geometries;

namespace Service.ManagerVPS.DTO.Output
{
    public class ParkingZoneItemOutput
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        
        public string? Owner { get; set; }
        public Models.ParkingZoneOwner? OwnerNavigation { get; set; }
        
        public DateTime? Created { get; set; }

        public bool? Status { get; set; }
        public Geometry? Location { get; set; } = null!;
        public string DetailAddress { get; set; } = null!;
    }
}
