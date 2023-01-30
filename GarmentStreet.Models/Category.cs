using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [ValidateNever]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
        [Required]
        [Display(Name = "Target Demographics")]
        public int TargetId { get; set; }
        [ForeignKey("TargetId")]
        [ValidateNever]
        public Target Target { get; set; }

    }
}
