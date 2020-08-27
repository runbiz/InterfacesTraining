using Interfaces.DAL.Contracts;
using Interfaces.Entities;
using Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DAL.Repositories
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(InterfacesContext context) : base(context) { }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.Name)
                .ToListAsync();
        }
    }
}
