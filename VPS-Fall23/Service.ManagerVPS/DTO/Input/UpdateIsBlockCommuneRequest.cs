namespace Service.ManagerVPS.DTO.Input;

public class UpdateIsBlockCommuneRequest
{
    public bool IsBlock { get; set; }
    public Guid CommuneId{ get; set; }
}