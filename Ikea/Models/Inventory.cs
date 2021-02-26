using System.ComponentModel.DataAnnotations;

namespace Ikea.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        [Required]
        public int ObjectId { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Furniture Furniture { get; set; }
        public virtual OrganizationalStructure Employee { get; set; }
    }
}
