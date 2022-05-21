using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class contextdb: IdentityDbContext<ApplicationUser>
    {
        public contextdb(DbContextOptions<contextdb> options):base(options)
        {
                
        }
        public DbSet<Employee> employees { get; set; }
        protected override void  OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.seed();
            foreach (var foriegnkey in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                foriegnkey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
