using System.Threading.Tasks;
using Orphee.CreationShared.Interfaces;

namespace Orphee.Models.Interfaces
{
    public interface IOrpheeFileManager
    {
        Task<string> ExportOrpheeFile(IOrpheeFile fileToSave);
        Task<IOrpheeFile> ImportOrpheeFile();
        void InitOrpheeFileWithImportedOrpheeFile(IOrpheeFile importedFile, IOrpheeFile fileToInitialize);
    }
}
