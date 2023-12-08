using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Service.ManagerVPS.Extensions.StaticLogic
{
    public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyConverter()
            : base(timeOnly => timeOnly.ToTimeSpan(), timeSpan => TimeOnly.FromTimeSpan(timeSpan))
        {

        }
    }
}
