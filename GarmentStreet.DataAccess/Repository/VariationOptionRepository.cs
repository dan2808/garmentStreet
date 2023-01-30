using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository
{
    public class VariationOptionRepository : Repository<VariationOption>, IVariationOptionRepository
    {
        public readonly ApplicationDbContext _db;
        public VariationOptionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(VariationOption obj)
        {
            //this updates all the properties even if they are not updated
            //_db.Categories.Update(category);

            var objFromDb = _db.VariationOptions.FirstOrDefault(c => c.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.VariationId = obj.VariationId;
            }
        }

        public IEnumerable<VariationOption> GetAllByVariationId(int id)
        {
            IEnumerable<VariationOption> listVariationOptions = _db.VariationOptions.Where(c => c.VariationId == id);

            return listVariationOptions;
        }
    }
}
