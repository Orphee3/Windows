using System.Collections.Generic;
using System.Threading.Tasks;
using Orphee.RestApiManagement.Models;

namespace Orphee.RestApiManagement.Getters.Interfaces
{
    public interface IPopularCreationGetter
    {
        Task<List<Creation>> GetpopularCreation();
    }
}
