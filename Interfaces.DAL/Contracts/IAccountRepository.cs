using Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace Interfaces.DAL.Contracts
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        IEnumerable<Account> AccountsByDeveloper(Guid developerId);
    }
}
