using System;
using System.Collections.Generic;

namespace Interfaces.Core.DTO
{
    public class DeveloperDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public IEnumerable<AccountDto> Accounts { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }
    }
}
