using System.ComponentModel.DataAnnotations;

namespace Ikea.Models
{
    public class OrganizationalStructure
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Department { get; set; }
        public int? BusinessUnit { get; set; }
    }
}
