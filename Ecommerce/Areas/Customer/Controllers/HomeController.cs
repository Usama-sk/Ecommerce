using DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Infrastructure.IRepository;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitofWork _unitofWork;

        public HomeController(ILogger<HomeController> logger, IUnitofWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitofWork.Product.GetAll(includeProperties: "Category");
            return View(products);

        }

        [HttpGet]
        public IActionResult Details(int? ProductId)
        {
            Cart cart = new Cart()
            {
                Product = _unitofWork.Product.GetT(x => x.ProductId == ProductId, includeProperties: "Category"),
                Count = 1,
                ProductId = (int)ProductId
            };

            return View(cart);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DeleteDetails(Cart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.AppUserId = claims.Value;
            var cartitem = _unitofWork.Cart.GetT(x => x.ProductId == cart.ProductId && x.AppUserId == claims.Value);

            _unitofWork.Cart.Delete(cartitem);
            _unitofWork.Save();

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cart.AppUserId = claims.Value;
                var cartitem = _unitofWork.Cart.GetT(x => x.ProductId == cart.ProductId && x.AppUserId == claims.Value);

                if (cartitem == null)
                {
                    _unitofWork.Cart.Add(cart);
                }
                else
                {
                    _unitofWork.Cart.IncrementCartItem(cartitem, cart.Count);

                }
                _unitofWork.Save();


            };

            return RedirectToAction("Index");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}