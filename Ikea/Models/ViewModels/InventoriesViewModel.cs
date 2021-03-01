using System.Collections.Generic;

namespace Ikea.Models.ViewModels
{
    public enum Filter
    {
        Employee,
        Department,
        BusinessUnit
    }
    public class InventoriesViewModel
    {
        public InventoriesViewModel()
        {
            Id = null;
        }
        public int? Id { get; set; }
        public Filter FilterType { get; set; }
        public List<OrganizationalStructure> Employee { get; set; }
        public List<OrganizationalStructure> Departments { get; set; }
        public List<OrganizationalStructure> BusinessUnits { get; set; }
        public List<Furniture> Furnitures { get; set; }
    }
}
