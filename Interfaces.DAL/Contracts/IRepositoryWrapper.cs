using Interfaces.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DAL.Contracts
{
    public interface IRepositoryWrapper
    {
        IDeveloperRepository Developer { get; }
        IAccountRepository Account { get; }
        IDepartmentRepository Department { get; }
        Task SaveAsync();
    }
}
