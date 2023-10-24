using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.FileManagement
{
    public class RemoveObjectsDto
    {
        public RemoveObjectsDto()
        {
            this.VersionsId = Array.Empty<string>();
        }
        [Required(AllowEmptyStrings = false)]
        public string ObjectName { get; set; } = null!;
        public string[]? VersionsId { get; set; }
        public Tuple<string, List<string>> ToTuple()
        {
            return Tuple.Create(ObjectName, VersionsId.ToList());
        }
    }
}
