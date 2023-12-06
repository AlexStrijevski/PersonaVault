using Azure.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Business.Services
{
    internal class ImageHandler : IImageHandler
    {
        public byte[] ConvertImageToByteArray(IFormFile image)
        {
            using var memoryStream = new MemoryStream();
            image.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public byte[] ResizeImageAndConvertToByteArray(IFormFile imageFile)
        {
            using var image = Image.FromStream(imageFile.OpenReadStream());
            using var resizedImage = new Bitmap(200,200);
            using var graphics = Graphics.FromImage(resizedImage);

            graphics.DrawImage(image, 0, 0, 200, 200);

            using var memoryStream = new MemoryStream();
            resizedImage.Save(memoryStream, image.RawFormat);

            return memoryStream.ToArray();
        }

        public bool DoesImageSizeMeetRequirements(IFormFile imageFile)
        {
            using var image = Image.FromStream(imageFile.OpenReadStream());

            if (image.Width == 200 && image.Height == 200) return true;

            return false;
        }
    }
}
