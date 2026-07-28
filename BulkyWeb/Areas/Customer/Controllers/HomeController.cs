using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "category");

            if (products == null)
            {
                throw new Exception("GetAll() returned null");
            }

            var list = products.ToList();

            return View(list);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Count = 1,
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "category"),
                ProductId = productId
            };

            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity!;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                shoppingCart.ApplicationUserId = userId;
                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ProductId == shoppingCart.ProductId && u.ApplicationUserId==shoppingCart.ApplicationUserId);

                if (cartFromDb != null)
                {
                    cartFromDb.Count = shoppingCart.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                    _unitOfWork.Save();
                    HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ProductId == shoppingCart.ProductId && u.ApplicationUserId == shoppingCart.ApplicationUserId).Count());
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);   // Logs the full exception

                return BadRequest(ex.Message);
            }
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
