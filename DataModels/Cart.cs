using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DataModels
{
    public class Cart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
        [ValidateNever]
        public string AppUserId { get; set; }
        [ValidateNever]
        public AppUser AppUser { get; set; }
        public int Count { get; set; }
    }
}
