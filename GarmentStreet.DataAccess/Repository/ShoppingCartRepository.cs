using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int quantity)
        {
            shoppingCart.Quatity -= quantity;
            return shoppingCart.Quatity;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int quantity)
        {
            shoppingCart.Quatity += quantity;
            return shoppingCart.Quatity;
        }
    }
}
