using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.DTO.Output;

public class ParkingZoneAndOwnerOutput
{
    public ParkingZone ParkingZone { get; set; } = null!;

    public ParkingZoneOwner Owner { get; set; } = null!;
    
    public int NumberOfParkingZones { get; set; }
}