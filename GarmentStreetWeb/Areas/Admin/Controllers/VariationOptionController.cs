
using GarmentStreet.DataAccess;
using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using GarmentStreet.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GarmentStreetWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VariationOptionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VariationOptionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            return View();
        }
        //GET
        public IActionResult Upsert(int? id)
        {
            VariationOptionViewModel variationOptionVM = new()
            {
                VariationOption = new(),
                VariationList = _unitOfWork.Variation.GetAll(includeProperties: "Category").Select(
                u => new SelectListItem
                {
                    Text = u.Name+" - "+u.Category.Name,
                    Value = u.Id.ToString()
                })
            };
            //IEnumerable<SelectListItem> TargetList = _unitOfWork.Target.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.TargetList = TargetList;
                //ViewData["TargetTypeList"] = TargetTypeList;
                return View(variationOptionVM);
            }
            else
            {
                //update product
                variationOptionVM.VariationOption = _unitOfWork.VariationOption.GetFirstOrDefault(u => u.Id == id);
                return View(variationOptionVM);
                
            }
            
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VariationOptionViewModel obj)
        {
            // custom server side validation 
            //if(obj.Name == "male")
            //{
            //    ModelState.AddModelError("Name", "name must be different from male");
            //}
            //server side validation


            if (ModelState.IsValid)
            {
                if (obj.VariationOption.Id == 0)
                {
                    _unitOfWork.VariationOption.Add(obj.VariationOption);
                    _unitOfWork.Save();
                    TempData["success"] = "Variation option created successfully";
                    return RedirectToAction("Index", "VariationOption");


                }
                else
                {
                    _unitOfWork.VariationOption.Update(obj.VariationOption);
                    _unitOfWork.Save();
                    TempData["success"] = "Variation option updated successfully";
                    return RedirectToAction("Index", "VariationOption");
                }

            }

            return View(obj);
        }

        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var variationOptionList = _unitOfWork.VariationOption.GetAll(includeProperties: "Variation.Category");
            return Json(new { data = variationOptionList });
        }

        [HttpGet]
        public IActionResult GetAllByVariationId(int id)
        {
            var variationList = _unitOfWork.VariationOption.GetAllByVariationId(id);
            return Json(new { data = variationList });
        }
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var variationOption = _unitOfWork.VariationOption.GetFirstOrDefault(x => x.Id == id, includeProperties: "Variation");
            return Json(new { data = variationOption });
        }

        //Post
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.VariationOption.GetFirstOrDefault(x => x.Id == id);
            if (objFromDbFirst == null)
            {
                return Json(new { sucess = false, message = "Error while deleting" });
            }
            _unitOfWork.VariationOption.Remove(objFromDbFirst);
            _unitOfWork.Save();
            return Json(new { sucess = false, message = "Delete successful" });
        }
        #endregion

    }
}
