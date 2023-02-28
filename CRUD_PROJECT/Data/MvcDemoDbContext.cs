using CRUD_PROJECT.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CRUD_PROJECT.Data
{
    public class MvcDemoDbContext :  DbContext
    {
        public MvcDemoDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Employee> Employees{ get; set; }
    }
}
