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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CompanyController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _UnitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
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
            // 1. Company -> because we're creating a new company.
            // 2. CategoryList -> to populate the category dropdown.
            //
            // Instead of sending multiple objects separately,
            // we wrap everything inside a single ViewModel.
            
            if(id==null || id ==0)
            // Send the ViewModel to the View.
            //create
            return View(new Company());
            else
            {
                Company companyObj = _UnitOfWork.Company.Get(U => U.Id==id);
                //update
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid) 
            {
                if (companyObj.Id==0)
                    _UnitOfWork.Company.Add(companyObj);
                else
                    _UnitOfWork.Company.Update(companyObj);
                _UnitOfWork.Save();
                TempData["Success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _UnitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _UnitOfWork.Company.Get(U => U.Id == id);
            if (companyToBeDeleted==null)
            {
                return Json(new { success = false, messsage = "Error while deleting" });

            }
            _UnitOfWork.Company.Remove(companyToBeDeleted);
            _UnitOfWork.Save();
            return Json(new { success=true });
        }
        #endregion
    }
}
