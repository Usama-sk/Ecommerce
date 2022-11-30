using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Category Description")]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
