using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.DTO.Input;

public class GetAddressListParkingZoneRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string TextAddress { get; set; } = "";
    public Guid? CityFilter { get;set; }
    public Guid? DistrictFilter { get;set; }

    public AddressTypeEnum TypeAddress { get; set; } = AddressTypeEnum.COMMUNE;
}