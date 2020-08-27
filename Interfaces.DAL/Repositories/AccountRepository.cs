using Interfaces.DAL.Contracts;
using Interfaces.Entities;
using Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interfaces.DAL.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {        
        public AccountRepository(InterfacesContext interfacesContext)
            : base(interfacesContext)
        {            
        }

        public IEnumerable<Account> AccountsByDeveloper(Guid developerId)
        {
            return FindByCondition(a => a.DeveloperId.Equals(developerId)).ToList();
        }
    }
}
