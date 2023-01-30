using GarmentStreet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository.IRepository
{
    public interface IVariationRepository : IRepository<Variation>
    {
        void Update(Variation variation);
        public IEnumerable<Variation> GetAllByCategoryId(int id);



    }
}
