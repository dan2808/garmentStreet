using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public IEnumerable<Category> GetAllByTargetId(int id)
        //{
        //    return _db.Categories.Where(x=>x.TargetId == id).ToList();
        //}

        public void Update(Category obj)
        {
            //this updates all the properties even if they are not updated
            //_db.Categories.Update(category);

            var objFromDb = _db.Categories.FirstOrDefault(c => c.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.TargetId = obj.TargetId;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
