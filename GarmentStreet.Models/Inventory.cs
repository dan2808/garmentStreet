using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [Required]
        public int VariationOptionId { get; set; }
        [ForeignKey("VariationOptionId")]
        
        [ValidateNever]
        public VariationOption VariationOption { get; set; }
    }
}
