using DataModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Infrastructure.IRepository;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitofWork _unitofWork;

        public CategoryController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Categories = _unitofWork.Category.GetAll();
            return View(categoryVM);
        }



        [HttpGet]
        public IActionResult CreateorUpdate(int? id)
        {
            CategoryVM vm = new CategoryVM();
            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Category = _unitofWork.Category.GetT(x => x.CategoryId == id);

                if (vm.Category == null)
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
        public IActionResult CreateorUpdate(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Category.CategoryId == null || vm.Category.CategoryId == 0)
                {
                    _unitofWork.Category.Add(vm.Category);
                    TempData["Success"] = "Category Created Done!";
                }
                else
                {
                    _unitofWork.Category.Update(vm.Category);
                    TempData["Success"] = "Category Updated Done!";
                }
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return View(vm.Category);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitofWork.Category.GetT(x => x.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var category = _unitofWork.Category.GetT(x => x.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitofWork.Category.Delete(category);
            _unitofWork.Save();
            TempData["Success"] = "Category Deleted Done!";
            return RedirectToAction("Index");
        }

    }
}
