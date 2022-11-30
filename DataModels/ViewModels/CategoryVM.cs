using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DataModels.ViewModels
{
    public class CategoryVM
    {
        public Category Category { get; set; } = new Category();
        [ValidateNever]
        public IEnumerable<Category> Categories { get; set; }
    }
}
