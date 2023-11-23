using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.GoongMap.AutoComplete
{
    public class Request
    {
        public Request()
        {
            MoreCompound = false;
            Limit = 20;
            Radius = 50;
        }
        [Required(AllowEmptyStrings = false)]
        public string Input { get; set; } = null!;
        public bool? MoreCompound { get; set; }
        public Position? Position { get; set; }
        public int? Limit { get; set; }
        public int? Radius { get; set; }
        public Guid SessionToken { get; set; }
    }
}
