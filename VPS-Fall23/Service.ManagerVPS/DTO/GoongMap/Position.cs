using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap
{
    public class Position
    {
        public Position()
        {

        }
        public Position(decimal lat, decimal @long)
        {
            Lat = lat;
            Lng = @long;
        }

        [JsonPropertyName("lat")]
        public decimal Lat { get; set; }
        [JsonPropertyName("lng")]
        public decimal Lng { get; set; }

        public NetTopologySuite.Geometries.Point GetTopologyPoint()
        {
            var point = new NetTopologySuite.Geometries.Point((double)Lng, (double)Lat);
            point.SRID = 4326;
            return point;
        }
    }
}
