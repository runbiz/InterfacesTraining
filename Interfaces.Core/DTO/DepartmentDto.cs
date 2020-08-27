using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Interfaces.Core.DTO
{
    public class DepartmentDto
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        

        // Relationships
        [JsonIgnore]
        public ICollection<DeveloperDto> Developers { get; set; }
    }
}
