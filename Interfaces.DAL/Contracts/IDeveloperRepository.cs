using Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.DAL.Contracts
{
    public interface IDeveloperRepository : IRepositoryBase<Developer>
    {
        Task<IEnumerable<Developer>> GetAllDevelopersAsync();
        Task<IEnumerable<Developer>> GetAllDevelopersWithDetailsAsync();
        Task<IEnumerable<Developer>> GetDevelopersByDepartmentAsync(int departmentId);
        Task<Developer> GetDeveloperByIdAsync(Guid developerId);
        Task<Developer> GetDeveloperWithDetailsAsync(Guid developerId);
        //void CreateDeveloper(Developer developer);
        //void UpdateDeveloper(Developer developer);
        //void DeleteDeveloper(Developer developer);
    }
}
