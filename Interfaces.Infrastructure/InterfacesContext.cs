using Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Interfaces.Infrastructure
{
    public class InterfacesContext : DbContext
    {
        public InterfacesContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
