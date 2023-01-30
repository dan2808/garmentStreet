using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository
{
    public class VariationRepository : Repository<Variation>, IVariationRepository
    {
        public readonly ApplicationDbContext _db;
        public VariationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Variation obj)
        {
            //this updates all the properties even if they are not updated
            //_db.Categories.Update(category);

            var objFromDb = _db.Variations.FirstOrDefault(c => c.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.CategoryId = obj.CategoryId;
            }
        }

        public IEnumerable<Variation> GetAllByCategoryId(int id)
        {
            IEnumerable<Variation> listVariation = _db.Variations.Where(c => c.CategoryId == id);

            return listVariation;
        }

    }
}
