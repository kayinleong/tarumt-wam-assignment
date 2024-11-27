namespace Tarumt.WAM.Assignment.Services
{
    public class FileService
    {
        private readonly string _filePath;

        public FileService(IWebHostEnvironment env)
        {
            _filePath = env.IsDevelopment()
                ? Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/files/")
                : @"/var/www/static/";

            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
        }

        public async Task<bool> UploadAsync(IFormFile file, string filename, string folderName = "public/")
        {
            string targetedFilePath = Path.Combine(_filePath, folderName);
            if (!Directory.Exists(targetedFilePath))
            {
                Directory.CreateDirectory(targetedFilePath);
            }

            try
            {
                await using FileStream stream = new(Path.Combine(targetedFilePath, filename), FileMode.Create);
                file.CopyTo(stream);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
