using System;
using System.ComponentModel.DataAnnotations;

namespace Interfaces.Core.DTO
{
    public class DeveloperForCreationDto
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters.")]
        public string Name { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "Address cannot be loner then 100 characters.")]
        public string Address { get; set; }

        // Relationships
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
    }
}
