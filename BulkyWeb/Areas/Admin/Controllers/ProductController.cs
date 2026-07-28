using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.Viewmodels;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "category").ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            //// Get all categories from the database.
            //// Convert each Category object into a SelectListItem because
            //// ASP.NET Core's <select> tag helper expects a collection of SelectListItem.
            //IEnumerable<SelectListItem> CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
            //{
            //    // Text = what the user sees in the dropdown
            //    Text = u.Name,

            //    // Value = what gets submitted when the form is posted
            //    Value = u.Id.ToString()
            //});

            // -----------------------------
            // Ways to pass data to a View
            // -----------------------------

            // ViewBag is a dynamic object.
            // No type checking at compile time.
            // Example:
            // ViewBag.CategoryList = CategoryList;

            // ViewData stores data as key-value pairs (Dictionary<string, object>).
            // Requires casting when reading the value.
            // Example:
            //ViewData["CategoryList"] = CategoryList;

            // -----------------------------
            // Create a ViewModel
            // -----------------------------

            // The Create page needs data from MORE THAN ONE model:
            // 1. Product -> because we're creating a new product.
            // 2. CategoryList -> to populate the category dropdown.
            //
            // Instead of sending multiple objects separately,
            // we wrap everything inside a single ViewModel.
            ProductVM productVM = new()
            {
                // Create a NEW Product object because the user is
                // creating a brand new product. The form fields
                // (Title, Price, Description, etc.) will bind to this object.
                Product = new Product(),

                // Get all categories from the database.
                // Convert each Category object into a SelectListItem because
                // ASP.NET Core's <select> tag helper expects a collection of SelectListItem.
                CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    // Text = what the user sees in the dropdown
                    Text = u.Name,

                    // Value = what gets submitted when the form is posted
                    Value = u.Id.ToString()
                })
            };
            if(id==null || id ==0)
            // Send the ViewModel to the View.
            //create
            return View(productVM);
            else
            {
                productVM.Product = _UnitOfWork.Product.Get(U => U.Id==id);
                //update
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                string ProductPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                {
                    //delete old image 
                    var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using(var filestream = new FileStream(Path.Combine(ProductPath, filename), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                productVM.Product.ImageUrl = @"\images\product\" + filename;
            }
            if (ModelState.IsValid) 
            {
                if (productVM.Product.Id == 0)
                    _UnitOfWork.Product.Add(productVM.Product);
                else
                    _UnitOfWork.Product.Update(productVM.Product);
                _UnitOfWork.Save();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "category").ToList();
            return Json(new { data = objProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {;
            var productToBeDeleted = _UnitOfWork.Product.Get(U => U.Id == id);
            if (productToBeDeleted==null)
            {
                return Json(new { success = false, messsage = "Error while deleting" });

            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _UnitOfWork.Product.Remove(productToBeDeleted);
            _UnitOfWork.Save();
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "category").ToList();
            return Json(new { data = objProductList });
        }
        #endregion
    }
}
