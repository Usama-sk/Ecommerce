﻿using DataModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepositoryLayer.Infrastructure.IRepository;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private IUnitofWork _unitofWork;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitofWork;
            _webHostEnvironment = webHostEnvironment;
        }
        #region API CALL
        public IActionResult AllProducts()
        {
            var products = _unitofWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = products });
        }
        #endregion
        public IActionResult Index()
        {
            //ProductVM productVM = new ProductVM();
            //productVM.Products = _unitofWork.Product.GetAll();
            return View();
        }



        [HttpGet]
        public IActionResult CreateorUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitofWork.Category.GetAll().Select(x =>
                 new SelectListItem()
                 {
                     Text = x.Name,
                     Value = x.CategoryId.ToString()

                 })
            };
            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Product = _unitofWork.Product.GetT(x => x.ProductId == id);

                if (vm.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);

                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateorUpdate(ProductVM vm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string fileName = String.Empty;
                if (file != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImage");
                    fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);
                    if (vm.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    vm.Product.ImageUrl = @"\ProductImage\" + fileName;

                }

                if (vm.Product.ProductId == 0)
                {
                    _unitofWork.Product.Add(vm.Product);
                    TempData["Success"] = "Product Created Done!";
                }
                else
                {
                    _unitofWork.Product.Update(vm.Product);
                    TempData["Success"] = "Product Updated Done!";
                }
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return View(vm.Product);
        }
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var product = _unitofWork.Product.GetT(x => x.ProductId == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}
        #region DeleteAPICALL
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitofWork.Product.GetT(x => x.ProductId == id);
            if (product == null)
            {
                return Json(new { success = false, Error = "Error in Fetching Data" });
            }
            else
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _unitofWork.Product.Delete(product);
                _unitofWork.Save();
                return Json(new { success = true, message = "Product Deleted" });

            }
        }
        #endregion
    }
}