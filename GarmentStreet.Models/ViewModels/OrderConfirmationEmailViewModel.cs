using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.Models.ViewModels
{
    public class OrderConfirmationEmailViewModel
    {
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public double OrderTotal { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }

    }
}
