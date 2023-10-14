namespace Service.ManagerVPS.DTO.Ouput;

public class RequestedParkingZoneItemOutput
{
    public Guid Id { get; set; }

    public int SubId { get; set; }

    public Guid CommuneId { get; set; }
    
    public string Name { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ModifiedAt { get; set; }

    public Guid OwnerId { get; set; }
    
    public string DetailAddress { get; set; }
    
    public string PricePerHour { get; set; }

    public string PriceOverTimePerHour { get; set; }

    public int? Slots { get; set; }
    
    public decimal? Lat { get; set; }
    
    public decimal? Lng { get; set; }
    
    public List<string> ParkingZoneImages {get; set; }
}