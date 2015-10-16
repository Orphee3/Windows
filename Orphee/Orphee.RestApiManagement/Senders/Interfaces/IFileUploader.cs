using System.Threading.Tasks;
using Windows.Storage;

namespace Orphee.RestApiManagement.Senders.Interfaces
{
    /// <summary>
    /// FileUploader interface
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// Sends an MIDI file to AWS S3 service
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <returns>Returns true if the request was sent and the response received correctly. Returns false otherwise</returns>
        Task<bool> UploadFile(StorageFile fileToUpload);

        /// <summary>
        /// Sends an image to AWS S3 service
        /// </summary>
        /// <param name="imageToUpload"></param>
        /// <returns>Returns true if the request was sent and the response received correctly. Returns false otherwise</returns>
        Task<bool> UploadImage(StorageFile imageToUpload);
    }
}
