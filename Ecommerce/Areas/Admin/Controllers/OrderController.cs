using DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Infrastructure.IRepository;
using System.Security.Claims;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area ("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private IUnitofWork _unitofWork;
        public OrderController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        #region API CALL
        public IActionResult AllOrders()
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole("Admin")|| User.IsInRole("Employee")) {

                orderHeaders = _unitofWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitofWork.OrderHeader.GetAll(x=>x.ApplicationUserId == claims.Value);

            }
            return Json(new { data = orderHeaders });
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
    }
}
