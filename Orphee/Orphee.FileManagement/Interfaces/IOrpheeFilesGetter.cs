using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Orphee.FileManagement.Interfaces
{
    public interface IOrpheeFilesGetter
    {
       Task<List<IStorageFile>> RetrieveOrpheeFiles();
    }
}
