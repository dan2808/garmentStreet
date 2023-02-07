using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public IEnumerable<Product> GetAllByCategoryId(int id)
        //{
        //    return _db.Products.Where(x => x.CategoryId == id).ToList();
        //}

        public void Update(Product obj)
        {
            //this updates all the properties even if they are not updated
            //_db.Categories.Update(category);

            var objFromDb = _db.Products.FirstOrDefault(c => c.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.CategoryId = obj.CategoryId;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
