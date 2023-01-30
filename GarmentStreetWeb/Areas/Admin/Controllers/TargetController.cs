
using GarmentStreet.DataAccess;
using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using Microsoft.AspNetCore.Mvc;

namespace GarmentStreetWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TargetController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public TargetController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Target> objTargetList = _unitOfWork.Target.GetAll();
            return View(objTargetList);
        }

        //GET
        public IActionResult Create()
        {

            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Target obj, IFormFile? file)
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
                    var uploads = Path.Combine(wwwRootPath, @"images\targets");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.ImageUrl = @"\images\targets\" + fileName + extension;
                }

                    _unitOfWork.Target.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Target created successfully";
                return RedirectToAction("Index", "Target");
            }

            return View(obj);

        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var targetfromDB = _db.Targets.Find(id);
            var targetFromDbFirst = _unitOfWork.Target.GetFirstOrDefault(x => x.Id == id);
            //var targetFromDbSingle = _db.Targets.SingleOrDefault(x => x.Id == id);

            if (targetFromDbFirst == null)
            {
                return NotFound();

            }

            return View(targetFromDbFirst);

        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Target obj, IFormFile? file)
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
                    var uploads = Path.Combine(wwwRootPath, @"images\targets");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.ImageUrl = @"\images\targets\" + fileName + extension;
                }


                _unitOfWork.Target.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Target updated successfully";
                return RedirectToAction("Index", "Target");
            }

            return View(obj);

        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var targetList = _unitOfWork.Target.GetAll();
            return Json(new { data = targetList });
        }
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var targetList = _unitOfWork.Target.GetFirstOrDefault(x => x.Id == id);
            return Json(new { data = targetList });
        }

        //Post
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Target.GetFirstOrDefault(x => x.Id == id);
            if (objFromDbFirst == null)
            {
                return Json(new { sucess = false, message = "Error while deleting" });
            }
            _unitOfWork.Target.Remove(objFromDbFirst);
            _unitOfWork.Save();
            return Json(new { sucess = false, message = "Delete successful" });
        }
        #endregion


    }
}
