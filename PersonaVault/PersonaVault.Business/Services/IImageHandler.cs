using Microsoft.AspNetCore.Http;

namespace PersonaVault.Business.Services
{
    public interface IImageHandler
    {
        byte[] ConvertImageToByteArray(IFormFile image);
        byte[] ResizeImageAndConvertToByteArray(IFormFile imageFile);
        bool DoesImageSizeMeetRequirements(IFormFile imageFile);
    }
}