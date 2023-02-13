
using GarmentStreet.DataAccess;
using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using GarmentStreet.Models.ViewModels;
using GarmentStreet.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GarmentStreetWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class VariationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VariationController(IUnitOfWork unitOfWork)
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
            VariationViewModel variationVM = new()
            {
                Variation = new(),
                CategoryList = _unitOfWork.Category.GetAll(includeProperties: "Target").Select(
                u => new SelectListItem
                {
                    Text = u.Name+" - "+u.Target.Name,
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
                return View(variationVM);
            }
            else
            {
                //update product
                variationVM.Variation = _unitOfWork.Variation.GetFirstOrDefault(u => u.Id == id);
                return View(variationVM);
                
            }
            
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VariationViewModel obj)
        {
            // custom server side validation 
            //if(obj.Name == "male")
            //{
            //    ModelState.AddModelError("Name", "name must be different from male");
            //}
            //server side validation


            if (ModelState.IsValid)
            {
                if (obj.Variation.Id == 0)
                {
                    _unitOfWork.Variation.Add(obj.Variation);
                    _unitOfWork.Save();
                    TempData["success"] = "Variation created successfully";
                    return RedirectToAction("Index", "Variation");


                }
                else
                {
                    _unitOfWork.Variation.Update(obj.Variation);
                    _unitOfWork.Save();
                    TempData["success"] = "Variation updated successfully";
                    return RedirectToAction("Index", "Variation");
                }

            }

            return View(obj);
        }

        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var variationList = _unitOfWork.Variation.GetAll(includeProperties: "Category");
            return Json(new { data = variationList });
        }

        [HttpGet]
        public IActionResult GetAllByCategoryId(int id)
        {
            var variationList = _unitOfWork.Variation.GetAllByCategoryId(id);
            return Json(new { data = variationList });
        }

        //Post
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Variation.GetFirstOrDefault(x => x.Id == id);
            if (objFromDbFirst == null)
            {
                return Json(new { sucess = false, message = "Error while deleting" });
            }
            _unitOfWork.Variation.Remove(objFromDbFirst);
            _unitOfWork.Save();
            return Json(new { sucess = false, message = "Delete successful" });
        }
        #endregion

    }
}
