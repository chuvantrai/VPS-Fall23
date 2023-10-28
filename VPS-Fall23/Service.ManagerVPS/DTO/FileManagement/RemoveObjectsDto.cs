using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.FileManagement
{
    public class RemoveObjectsDto
    {
        [Required(AllowEmptyStrings = false)]
        public string ObjectName { get; set; } = null!;
        
        public string[]? VersionsId { get; set; }
    }
}