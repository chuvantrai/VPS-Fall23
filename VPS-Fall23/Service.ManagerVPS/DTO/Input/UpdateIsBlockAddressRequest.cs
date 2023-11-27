using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.DTO.Input;

public class UpdateIsBlockAddressRequest
{
    public bool IsBlock { get; set; }
    public Guid CommuneId{ get; set; }
    public AddressTypeEnum TypeAddress { get; set; } = AddressTypeEnum.COMMUNE;
}