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
    public class VariationOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public int VariationId { get; set; }
        [ForeignKey("VariationId")]
        [ValidateNever]
        public Variation Variation { get; set; }
    }
}
