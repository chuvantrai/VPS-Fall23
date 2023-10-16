namespace Service.ManagerVPS.DTO.Ouput;

public class RequestedParkingZoneItemOutput
{
    public Guid Id { get; set; }

    public int Key { get; set; }

    public Guid CommuneId { get; set; }

    public string Name { get; set; } = null!;

    public string CreatedAt { get; set; } = null!;

    public string ModifiedAt { get; set; } = null!;

    public Guid OwnerId { get; set; }

    public string DetailAddress { get; set; } = null!;

    public string PricePerHour { get; set; } = null!;

    public string PriceOverTimePerHour { get; set; } = null!;

    public int? Slots { get; set; }

    public decimal? Lat { get; set; }

    public decimal? Lng { get; set; }

    public List<string> ParkingZoneImages { get; set; } = null!;
}