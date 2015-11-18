using System.Threading.Tasks;
using Windows.Storage;

namespace Orphee.CreationShared.Interfaces
{
    public interface IFilePickerManager
    {
        Task<StorageFile> GetTheSaveFilePicker(IOrpheeFile orpheeFile);
    }
}
