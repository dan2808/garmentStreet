using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TargetList { get; set; }

    }
}
