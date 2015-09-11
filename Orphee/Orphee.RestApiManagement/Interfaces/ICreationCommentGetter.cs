using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;
using Orphee.RestApiManagement.Annotations;

namespace Orphee.RestApiManagement
{
    public interface ICreationCommentGetter
    {
        Task<List<Comment>> GetCreationComments(string creationId);
    }
}
