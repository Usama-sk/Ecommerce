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

        public IActionResult Summary()
        {
            return View();
        }
            public IActionResult plus(int id)
        {   
            var cart = _unitofWork.Cart.GetT(x => x.CartId == id);
            _unitofWork.Cart.IncrementCartItem(cart , 1);
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int id)
        {
            var cart = _unitofWork.Cart.GetT(x => x.CartId == id);
            if(cart.Count <= 1)
            {
                _unitofWork.Cart.Delete(cart);
            }
            else
            {
                _unitofWork.Cart.DecrementCartItem(cart, 1);
                
            }
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
          
            var cart = _unitofWork.Cart.GetT(x => x.CartId == id);
            _unitofWork.Cart.Delete(cart);
            _unitofWork.Save();

            return RedirectToAction(nameof (Index));
        }
    }
}
