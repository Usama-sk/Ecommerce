using DataModels;
using DataModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Infrastructure.IRepository;
using System.Security.Claims;

namespace Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CartVM CartItemsList { get; set; }

        public CartController(IUnitofWork unitofWork)
        {
            
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
          
            CartItemsList = new CartVM
            {
                Carts = _unitofWork.Cart.GetAll(x=>x.AppUserId == claims.Value,includeProperties:"Product")
              
            };

            foreach (var item in CartItemsList.Carts)
            {
                CartItemsList.Total += (item.Product.Price * item.Count); 
            }

            return View(CartItemsList);
        }
    }
}
