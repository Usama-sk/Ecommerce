using CommonHelpers;
using DataModels;
using DataModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Infrastructure.IRepository;
using Stripe.Checkout;
using System.Security.Claims;

namespace Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CartVM vm { get; set; }

        public CartController(IUnitofWork unitofWork)
        {
            
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            vm = new CartVM
            {
                Carts = _unitofWork.Cart.GetAll(x => x.AppUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
              
            };
         
            foreach (var item in vm.Carts)
            {
                vm.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            vm = new CartVM
            {
                Carts = _unitofWork.Cart.GetAll(x => x.AppUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()

            };
            vm.OrderHeader.ApplicationUser = _unitofWork.AppUser.GetT(x => x.Id == claims.Value);
            vm.OrderHeader.Name = vm.OrderHeader.ApplicationUser.Name;
            vm.OrderHeader.Phone = vm.OrderHeader.ApplicationUser.PhoneNumber;
            vm.OrderHeader.Address = vm.OrderHeader.ApplicationUser.Address;
            vm.OrderHeader.City = vm.OrderHeader.ApplicationUser.City;
            vm.OrderHeader.State = vm.OrderHeader.ApplicationUser.State;
            vm.OrderHeader.PostalCode = vm.OrderHeader.ApplicationUser.PinCode;

            foreach (var item in vm.Carts)
            {
                vm.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }


            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Summary(CartVM vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        
            vm.Carts = _unitofWork.Cart.GetAll(x => x.AppUserId == claims.Value, includeProperties: "Product");
            vm.OrderHeader.OrderStatus = OrderStatus.StatusPending;
            vm.OrderHeader.PaymentStatus = PaymentStatus.StatusPending;
            vm.OrderHeader.DateofPayment = DateTime.Now;
            vm.OrderHeader.ApplicationUserId = claims.Value;
            foreach (var item in vm.Carts)
            {
                vm.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }
            _unitofWork.OrderHeader.Add(vm.OrderHeader);
            _unitofWork.Save();
            foreach (var item in vm.Carts)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = vm.OrderHeader.Id,
                    Count = item.Count,
                    Price = item.Product.Price

                };
                _unitofWork.OrderDetail.Add(orderDetail);
                _unitofWork.Save();
            }
            var domain = "https://localhost:44385/";
            var options = new SessionCreateOptions
            {
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            SuccessUrl = domain+$"customer/cart/ordersuccess?id={vm.OrderHeader.Id}",
            CancelUrl = domain + $"customer/cart/Index",
            };
            foreach (var item in vm.Carts)
            {

                var lineitemsOptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.Product.Price *100,
                        Currency = "PKR",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(lineitemsOptions);    
            

            }

            var  service = new SessionService();
            Session session = service.Create(options);
            _unitofWork.OrderHeader.PaymentStatus(vm.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitofWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            _unitofWork.Cart.DeleteRange(vm.Carts);
            _unitofWork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult ordersuccess(int id)
        {
            var orderHeader = _unitofWork.OrderHeader.GetT(x => x.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if(session.PaymentStatus.ToLower() == "paid")
            {
                _unitofWork.OrderHeader.UpdateStatus(id, OrderStatus.StatusApproved, PaymentStatus.StatusApproved);
            }
            List<Cart> cart = _unitofWork.Cart.GetAll(x=>x.AppUserId == orderHeader.ApplicationUserId).ToList();
            _unitofWork.Cart.DeleteRange(cart);
            _unitofWork.Save();
            return View(id);
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
