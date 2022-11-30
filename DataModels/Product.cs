using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Product Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Product Price")]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Product Image")]
        [ValidateNever]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
