using Interfaces.DAL.Contracts;
using Interfaces.Entities;
using Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interfaces.DAL.Repositories
{
    public class DeveloperRepository : RepositoryBase<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(InterfacesContext interfacesContext) : base(interfacesContext)
        {
        }

        public async Task<IEnumerable<Developer>> GetAllDevelopersAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Developer>> GetAllDevelopersWithDetailsAsync()
        {
            return await FindAll()
                .Include(d => d.Accounts)
                .Include(d => d.Department)
                .OrderBy(ow => ow.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Developer>> GetDevelopersByDepartmentAsync(int departmentId)
        {
            return await FindByCondition(d => d.DepartmentId.Equals(departmentId))
                .Include(d => d.Department)
                .ToListAsync();
        }        

        public async Task<Developer> GetDeveloperByIdAsync(Guid developerId)
        {
            return await FindByCondition(d => d.Id.Equals(developerId))
                .FirstOrDefaultAsync();
        }

        public async Task<Developer> GetDeveloperWithDetailsAsync(Guid developerId)
        {
            return await FindByCondition(d => d.Id.Equals(developerId))
                .Include(d => d.Accounts)
                .Include(d => d.Department)
                .FirstOrDefaultAsync();
        }

        //public void CreateDeveloper(Developer developer)
        //{
        //    Create(developer);
        //}

        //public void UpdateDeveloper(Developer developer)
        //{
        //    Update(developer);
        //}

        //public void DeleteDeveloper(Developer developer)
        //{
        //    Delete(developer);
        //}
    }
}
