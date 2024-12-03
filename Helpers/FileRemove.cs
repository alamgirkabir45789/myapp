using System.IO;

namespace myportfolio.Helpers
{
    public class FileRemove
    {
        public static void ImageRemove(string imagePath)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
