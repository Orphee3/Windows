using System.Threading.Tasks;
using Windows.Storage;

namespace Orphee.RestApiManagement.Interfaces
{
    public interface IFileUploader
    {
        Task<bool> UploadFile(StorageFile fileToUpload);
        Task<bool> UploadImage(StorageFile imageToUpload);
    }
}
