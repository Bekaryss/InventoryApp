using Ikea.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ikea.Models
{
    public interface IInventoryService
    {
        public Task<InventoriesViewModel> GetFurnituresAsync(int? id, Filter filter);
        public Task<IEnumerable<OrganizationalStructure>> GetEmployeesAsync();
    }
}