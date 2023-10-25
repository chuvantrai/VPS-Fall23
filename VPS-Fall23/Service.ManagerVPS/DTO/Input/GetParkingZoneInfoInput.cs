using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class GetParkingZoneInfoInput
{
    [Required] 
    public Guid? ParkingZoneId { get; set; }
}