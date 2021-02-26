using System.ComponentModel.DataAnnotations;

namespace Ikea.Models
{
    public class Furniture
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}
