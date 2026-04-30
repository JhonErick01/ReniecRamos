namespace ReniecRamos.Infraestructure
{
    public class FileStorage
    {
        private readonly IWebHostEnvironment _env;

        public FileStorage(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0) return null;
            // Ruta: wwwroot/uploads/citizens
            var uploads = Path.Combine(_env.WebRootPath, "uploads", folder);

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
