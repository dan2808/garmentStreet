using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        public readonly ApplicationDbContext _db;
        public InventoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Inventory obj)
        {
            //this updates all the properties even if they are not updated
            //_db.Categories.Update(category);

            var objFromDb = _db.Inventories.FirstOrDefault(c => c.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Quantity = obj.Quantity;
                objFromDb.ProductId = obj.ProductId;
                objFromDb.VariationOptionId = obj.VariationOptionId;
            }
        }

        //public IEnumerable<Variation> FindVariationByCategoryId(int id)
        //{
        //    IEnumerable<Variation> listVariation = _db.Variations.Where(c => c.CategoryId == id);

        //    return listVariation;
        //}

        //public IEnumerable<Inventory> GetAllByProductId(int id, string? includeProperties = null)
        //{
        //    IQueryable<Inventory> query = _db.Inventories.Where(x => x.ProductId == id);
        //    if (includeProperties != null)
        //    {
        //        foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            query = query.Include(includeProp);

        //        }
        //    }
        //    return query.ToList();

        //}
    }
}
