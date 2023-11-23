namespace Service.ManagerVPS.DTO.Input;

public class GetAddressListParkingZoneRequest
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}