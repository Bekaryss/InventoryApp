using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ikea.Models
{
    public class OrganizationalStructure : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Department { get; set; }
        public int? BusinessUnit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Department != null && BusinessUnit != null)
            {
                yield return new ValidationResult($"Department and BusinessUnit cannot be filled together.");
            }
        }
    }
}
