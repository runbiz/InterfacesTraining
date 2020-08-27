using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Interfaces.Entities
{
    [Table("Departments")]
    public class Department
    {
        [Column("DepartmentId")]
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Name must be less than 200 characters.")]
        public string Name { get; set; }        
        public string Description { get; set; }
        public bool Deleted { get; set; }

        // Relationships
        public ICollection<Developer> Developers { get; set; }
    }
}
