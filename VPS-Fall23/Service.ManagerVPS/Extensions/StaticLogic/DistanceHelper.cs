

using Service.ManagerVPS.DTO.GoongMap;

namespace Service.ManagerVPS.Extensions.StaticLogic
{
    public static class DistanceHelper
    {
        public static double CalculateDistance(this Position position, Position positionB)
        {
            return Math.Acos(Math.Sin((double)position.Lat) * Math.Sin((double)positionB.Lat) + Math.Cos((double)position.Lat) * Math.Cos((double)positionB.Lat) * Math.Cos((double)(positionB.Lng - position.Lng))) * 6371;
        }
    }
}
