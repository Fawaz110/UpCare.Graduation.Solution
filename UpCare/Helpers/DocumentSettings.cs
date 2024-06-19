namespace UpCare.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get located folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", folderName);

            // 2. Get file name and make it unique
            string fileName = $"{Guid.NewGuid()}-{file.FileName}";

            // 3. Get file path
            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save file as streams [data per time]
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", folderName, fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
