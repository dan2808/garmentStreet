
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

    public class InventoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InventoryController(IUnitOfWork unitOfWork)
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
            InventoryViewModel inventoryVM = new()
            {
                Inventory = new(),
                ProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").Select(
                u => new SelectListItem
                {
                    Text = u.Name, //+" - "+u.Target.Name,
                    Value = u.Id.ToString()
                }),
                VariationOptionList = _unitOfWork.VariationOption.GetAll(includeProperties: "Variation").Select(
                u => new SelectListItem
                {
                    Text = u.Name, //+" - "+u.Target.Name,
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
                return View(inventoryVM);
            }
            else
            {
                //update product
                inventoryVM.Inventory = _unitOfWork.Inventory.GetFirstOrDefault(u => u.Id == id);
                return View(inventoryVM);

            }

        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(InventoryViewModel obj)
        {
            // custom server side validation 
            //if(obj.Name == "male")
            //{
            //    ModelState.AddModelError("Name", "name must be different from male");
            //}
            //server side validation


            if (ModelState.IsValid)
            {
                if (obj.Inventory.Id == 0)
                {
                    _unitOfWork.Inventory.Add(obj.Inventory);
                    _unitOfWork.Save();
                    TempData["success"] = "Inventory item created successfully";
                    return RedirectToAction("Index", "Inventory");


                }
                else
                {
                    _unitOfWork.Inventory.Update(obj.Inventory);
                    _unitOfWork.Save();
                    TempData["success"] = "Inventory item updated successfully";
                    return RedirectToAction("Index", "Inventory");
                }

            }

            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var inventoryList = _unitOfWork.Inventory.GetAll(includeProperties: "Product,VariationOption");
            return Json(new { data = inventoryList });
        }

        //Post
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Inventory.GetFirstOrDefault(x => x.Id == id);
            if (objFromDbFirst == null)
            {
                return Json(new { sucess = false, message = "Error while deleting" });
            }
            _unitOfWork.Inventory.Remove(objFromDbFirst);
            _unitOfWork.Save();
            return Json(new { sucess = false, message = "Delete successful" });
        }
        #endregion

    }
}
