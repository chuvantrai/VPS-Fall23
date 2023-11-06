using System.Security.Claims;
using System.Text.RegularExpressions;
using Service.ManagerVPS.DTO.OtherModels;

namespace Service.ManagerVPS.Extensions.StaticLogic;

public static class GeneralExtension
{
    public static string GetPathService(string nameService)
    {
        var currentPath = Directory.GetCurrentDirectory();
        var parentPath = Directory.GetParent(currentPath)!.FullName;
        var targetPath = Path.Combine(parentPath, nameService);
        return targetPath;
    }

    public static UserTokenHeader? ConvertClaimToUserToken(IEnumerable<Claim> claims)
    {
        try
        {
            var arrayClaims = claims as Claim[] ?? claims.ToArray();
            var userToken = new UserTokenHeader()
            {
                UserId = arrayClaims.FirstOrDefault(c => c.Type == nameof(UserTokenHeader.UserId))!.Value,
                FirstName = arrayClaims.FirstOrDefault(c => c.Type == nameof(UserTokenHeader.FirstName))!.Value,
                LastName = arrayClaims.FirstOrDefault(c => c.Type == nameof(UserTokenHeader.LastName))!.Value,
                Email = arrayClaims.FirstOrDefault(c => c.Type == nameof(UserTokenHeader.Email))!.Value,
                RoleId = int.Parse(arrayClaims.FirstOrDefault(c => c.Type == nameof(UserTokenHeader.RoleId))!.Value),
                RoleName = arrayClaims.FirstOrDefault(c => c.Type == nameof(UserTokenHeader.RoleName))!.Value,
                Avatar = arrayClaims.FirstOrDefault(c => c.Type == nameof(UserTokenHeader.Avatar))!.Value,
                Expires = DateTime.Parse(arrayClaims.FirstOrDefault(c =>
                    c.Type == nameof(UserTokenHeader.Expires))!.Value),
                ModifiedAt = DateTime.Parse(arrayClaims.FirstOrDefault(c =>
                    c.Type == nameof(UserTokenHeader.ModifiedAt))!.Value)
            };
            return userToken;
        }
        catch
        {
            return null;
        }
    }

    public static bool CheckEqualDateTime(DateTime dateTime1 ,DateTime dateTime2)
    {
        try
        {
            return dateTime1.ToString("yyyy-MM-dd HH:mm:ss") == dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
        }
        catch 
        {
            return false;
        }
    }

    public static bool IsLicensePlateValid(string licensePlate)
    {
        string pattern = @"^[A-Za-z0-9\-\.]{8-9}+$";
        return Regex.IsMatch(licensePlate, pattern);
    }


}