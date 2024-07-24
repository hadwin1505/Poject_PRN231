using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StudentSupervisorService.CloudinaryConfig;
using static System.Net.Mime.MediaTypeNames;

namespace StudentSupervisorService.Service.Implement
{
    public class ImageUrlImplement : ImageUrlService
    {
        private readonly Cloudinary cloudinary;
        public ImageUrlImplement(IOptions<CloudinarySetting> setting)
        {
            var account = new Account(setting.Value.CloudName, setting.Value.ApiKey, setting.Value.ApiSecret);
            cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile image)
        {
            try
            {
                var uploadResult = new ImageUploadResult();
                if (image.Length > 0)
                {
                    using var stream = image.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, stream),
                        Transformation = new Transformation()
                            .Width(1000).Crop("scale").Chain()
                            .Quality(50).Chain()
                            .FetchFormat("auto")
                    };
                    uploadResult = await cloudinary.UploadAsync(uploadParams);
                }
                return uploadResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Tải hình ảnh lên không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var result = await cloudinary.DestroyAsync(deleteParams);
                return result;
            }
            catch (Exception ex)
                       {
                throw new Exception("Xóa hình ảnh không thành công: " + ex.Message
                                       + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }
    }
}
