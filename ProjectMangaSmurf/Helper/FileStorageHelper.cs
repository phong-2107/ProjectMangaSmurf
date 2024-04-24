namespace ProjectMangaSmurf.Helper
{
    public static class FileStorageHelper
    {
        private static readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        // Ensures the storage directory exists
        static FileStorageHelper()
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        // Method to save an image file and return the path
        public static async Task<string> SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("No file provided or file is empty.");

            // Generate a unique file name to prevent overwriting existing files
            string fileName = GenerateUniqueFileName(file.FileName);
            var filePath = Path.Combine(_storagePath, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the path relative to the wwwroot to store in the database
            return $"/images/{fileName}";
        }

        // Helper to generate a unique file name
        private static string GenerateUniqueFileName(string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName);
            string fileName = Path.GetFileNameWithoutExtension(originalFileName);
            string uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
            return uniqueFileName;
        }
    }

}
