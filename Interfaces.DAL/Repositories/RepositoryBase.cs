using Interfaces.DAL.Contracts;
using Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Interfaces.DAL.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected InterfacesContext InterfacesContext { get; set; }
        
        public RepositoryBase(InterfacesContext interfacesContext)
        {
            this.InterfacesContext = interfacesContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.InterfacesContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.InterfacesContext.Set<T>()
                .Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.InterfacesContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.InterfacesContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.InterfacesContext.Set<T>().Remove(entity);
        }
    }
}
