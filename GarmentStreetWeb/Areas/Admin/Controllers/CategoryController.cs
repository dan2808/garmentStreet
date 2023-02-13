
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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {

            return View();
        }
        //GET
        public IActionResult Upsert(int? id)
        {
            CategoryViewModel categoryVM = new()
            {
                Category = new(),
                TargetList = _unitOfWork.Target.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
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
                return View(categoryVM);
            }
            else
            {
                //update product
                categoryVM.Category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
                return View(categoryVM);
            }

        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CategoryViewModel obj, IFormFile? file)
        {
            // custom server side validation 
            //if(obj.Name == "male")
            //{
            //    ModelState.AddModelError("Name", "name must be different from male");
            //}
            //server side validation

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\category");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Category.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Category.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Category.ImageUrl = @"\images\category\" + fileName + extension;
                }

                if (obj.Category.Id == 0)
                {
                    _unitOfWork.Category.Add(obj.Category);
                    _unitOfWork.Save();
                    TempData["success"] = "Category created successfully";
                    return RedirectToAction("Index", "Category");


                }
                else
                {
                    _unitOfWork.Category.Update(obj.Category);
                    _unitOfWork.Save();
                    TempData["success"] = "Category updated successfully";
                    return RedirectToAction("Index", "Category");
                }
                
            }
            return View(obj);
        }



        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var categoryList = _unitOfWork.Category.GetAll(includeProperties: "Target");
            return Json(new { data = categoryList });
        }

        //Post
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (objFromDbFirst == null)
            {
                return Json(new { sucess = false, message = "Error while deleting" });
            }
            _unitOfWork.Category.Remove(objFromDbFirst);
            _unitOfWork.Save();
            return Json(new { sucess = false, message = "Delete successful" });
        }
        #endregion

    }
}
