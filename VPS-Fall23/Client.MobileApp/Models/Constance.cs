using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MobileApp.Models
{
    public static class Constance
    {
        public static string GoogleAppCredentials = "GoogleAppCredentials.json";
        public static string ImageName = "Image " + DateTime.Now.ToString();
        public static async Task CopyFileToAppDataDirectory(string filename)
        {
            if (!String.IsNullOrEmpty(filename))
            {
                using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
                StreamReader streamReader = new StreamReader(inputStream);
                var content = streamReader.ReadToEnd();
                string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
                File.WriteAllText(targetFile, content);
            }
            else
            {
                throw new Exception("File Not Found");
            }
        }
    }
}
