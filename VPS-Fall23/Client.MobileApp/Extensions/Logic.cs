using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MobileApp.Extensions
{
    public static class Logic
    {
        public static async Task CopyFileToAppDataDirectory(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
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
