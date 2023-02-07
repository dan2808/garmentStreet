using GarmentStreet.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Target = new TargetRepository(_db);
            Category = new CategoryRepository(_db);
            Variation = new VariationRepository(_db);
            VariationOption = new VariationOptionRepository(_db);
            Product = new ProductRepository(_db);
            Inventory = new InventoryRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
        }
        public ITargetRepository Target { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IVariationRepository Variation { get; private set; }
        public IVariationOptionRepository VariationOption { get; private set; }
        public IProductRepository Product { get; private set; }
        public IInventoryRepository Inventory { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }


        public void Save() 
        {
            _db.SaveChanges();
        }
    }
}
