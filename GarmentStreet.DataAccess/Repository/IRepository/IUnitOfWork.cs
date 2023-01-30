using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ITargetRepository Target { get; }
        ICategoryRepository Category { get; }
        IVariationRepository Variation { get; }
        IVariationOptionRepository VariationOption { get; }
        IProductRepository Product { get; }
        IInventoryRepository Inventory { get; }

        public void Save();
    }
}
