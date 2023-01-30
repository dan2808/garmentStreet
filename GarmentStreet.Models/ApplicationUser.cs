using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(60)]
        public string? StreetAddress { get; set; }
        [MaxLength(20)]
        public string? City { get; set; }
        [MaxLength(10)]
        public string? State { get; set; }
        [MaxLength(10)] 
        public string? PostalCode { get; set; }
    }
}
