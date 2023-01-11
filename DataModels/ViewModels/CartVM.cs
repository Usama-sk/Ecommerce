using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.ViewModels
{
    public class CartVM
    {
        [ValidateNever]
        public IEnumerable<Cart> Carts { get; set; }

        public double Total { get; set; }

    }
}
