using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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
            IEnumerable<Inventory> inventoryList = _unitOfWork.Inventory.GetAllByProductId(id,"Product,VariationOption");
            return View(inventoryList);
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