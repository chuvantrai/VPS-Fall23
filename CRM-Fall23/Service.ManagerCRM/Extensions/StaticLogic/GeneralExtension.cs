namespace Service.ManagerCRM.Extensions.StaticLogic;

public static class GeneralExtension
{
    public static string GetPathService(string nameService)
    {
        var currentPath = Directory.GetCurrentDirectory();
        var parentPath = Directory.GetParent(currentPath)!.FullName;
        var targetPath = Path.Combine(parentPath, nameService);
        return targetPath;
    }
}