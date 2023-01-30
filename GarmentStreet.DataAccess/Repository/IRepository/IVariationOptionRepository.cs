using GarmentStreet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository.IRepository
{
    public interface IVariationOptionRepository : IRepository<VariationOption>
    {
        void Update(VariationOption variationOption);

        public IEnumerable<VariationOption> GetAllByVariationId(int id);


    }
}
