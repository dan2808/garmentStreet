using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using GarmentStreet.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;

namespace GarmentStreetWeb.Areas.Customer.Controllers
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
            IEnumerable<Target> targetList = _unitOfWork.Target.GetAll();
            return View(targetList);
        }

        public IActionResult Categories(int id)
        {
            IEnumerable<Category> categoryList = _unitOfWork.Category.GetAllByTargetId(id);
            return View(categoryList);
        }


        public IActionResult Products(int id)
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAllByCategoryId(id);
            return View(productList);
        }

        public IActionResult Item(int id)
        {
            ShoppingCartViewModel shoppingCartVM = new()
            {
                ShoppingCart = new()
                { Quatity = 1},
                Product = _unitOfWork.Product.GetFirstOrDefault(u=>u.Id == id),
                InventoryList = _unitOfWork.Inventory.GetAllByProductId(id, "Product,VariationOption").Select(
                u => new SelectListItem
                {
                    Text = u.VariationOption.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(shoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Item(ShoppingCartViewModel shoppingCartViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartViewModel.ShoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u=>u.ApplicationUserId == claim.Value && u.InventoryId == shoppingCartViewModel.ShoppingCart.InventoryId);

            if(cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCartViewModel.ShoppingCart);

            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, shoppingCartViewModel.ShoppingCart.Quatity);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Products),new { id = shoppingCartViewModel.Product.CategoryId });
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