using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Claims;

namespace BulkyWeb.ViewComponents
{
    public class ShoppingCartViewComponent:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Clear();
                return View(0);
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return View(0);
            }

            if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
            {
                int cartCount = _unitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == claim.Value)
                    .Count();

                HttpContext.Session.SetInt32(SD.SessionCart, cartCount);
            }

            return View(HttpContext.Session.GetInt32(SD.SessionCart) ?? 0);
        }
    }
}
