using GarmentStreet.DataAccess.Repository.IRepository;
using GarmentStreet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.DataAccess.Repository
{
    public class TargetRepository : Repository<Target>, ITargetRepository
    {
        public readonly ApplicationDbContext _db;
        public TargetRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Target target)
        {
            _db.Targets.Update(target);
        }
    }
}
