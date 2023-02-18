using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using GarmentStreet.Utility;
using Microsoft.AspNetCore.Mvc;

namespace GarmentStreetWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var id = 1;
            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == id, includeProperties: "ApplicationUser");

            
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            IEnumerable<Category> categoryList = _unitOfWork.Category.GetAll();
            return Json(new { data = categoryList });
        }
        [HttpGet]
        public IActionResult GetVariationByCategoryId(int id)
        {
            IEnumerable<Variation> variationList = _unitOfWork.Variation.GetAllByCategoryId(id);
            return Json(new { data = variationList });
        }
        #endregion
    }
}
