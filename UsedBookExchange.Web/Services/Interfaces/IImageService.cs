using Microsoft.AspNetCore.Http;

namespace UsedBookExchange.Web.Services.Interfaces;

public interface IImageService
{
    Task<string?> SaveImageAsync(IFormFile image);

void DeleteImage(string? imagePath);

}
