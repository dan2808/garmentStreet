using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
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
