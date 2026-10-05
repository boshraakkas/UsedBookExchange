using UsedBookExchange.Web.Services.Interfaces;

namespace UsedBookExchange.Web.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

private static readonly string[] AllowedExtensions =
{
    ".jpg",
    ".jpeg",
    ".png",
    ".webp"
};

    private const long MaxFileSize = 5 * 1024 * 1024;

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string?> SaveImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
            return null;

        if (image.Length > MaxFileSize)
            throw new InvalidOperationException(
                "Image size cannot exceed 5 MB.");

        var extension =
            Path.GetExtension(image.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException(
                "Only JPG, JPEG, PNG and WEBP images are allowed.");

        var uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "books");

        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName);

        await using var stream = new FileStream(
            filePath,
            FileMode.Create);

        await image.CopyToAsync(stream);

        return $"/uploads/books/{fileName}";
    }

    public void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return;

        var fileName = Path.GetFileName(imagePath);

        if (string.IsNullOrWhiteSpace(fileName))
            return;

        var filePath = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "books",
            fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }

}
