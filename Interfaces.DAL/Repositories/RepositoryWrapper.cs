using Interfaces.DAL.Contracts;
using Interfaces.Infrastructure;
using System.Threading.Tasks;

namespace Interfaces.DAL.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private InterfacesContext _context;
        private IDeveloperRepository _developer;
        private IAccountRepository _account;
        private IDepartmentRepository _department;
        public RepositoryWrapper(InterfacesContext context)
        {
            _context = context;
        }

        public IDeveloperRepository Developer
        {
            get
            {
                if (_developer == null)
                {
                    _developer = new DeveloperRepository(_context);
                }
                return _developer;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_context);
                }
                return _account;
            }
        }

        public IDepartmentRepository Department
        {
            get
            {
                if (_department == null)
                {
                    _department = new DepartmentRepository(_context);
                }
                return _department;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
