using System.ComponentModel.DataAnnotations.Schema;

namespace Ikea.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }

    }
}
