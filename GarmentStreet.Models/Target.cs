using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GarmentStreet.Models
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [DisplayName("Name")]
        public string Name { get; set; }
        [ValidateNever]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
