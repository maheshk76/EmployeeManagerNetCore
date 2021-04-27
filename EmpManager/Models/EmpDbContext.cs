
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EmpManager.Models
{
    public class EmpDbContext : DbContext
    {
        public EmpDbContext()
        {

        }
        public EmpDbContext(DbContextOptions<EmpDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeToProject> EmployeeToProjects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmpProjectListViewModel>().HasNoKey();
        }
        public async Task<List<EmpProjectListViewModel>> GetData(string sql,List<object> para)
        {
            // Initialization.  
            List<EmpProjectListViewModel> lst = new List<EmpProjectListViewModel>();

            try
            {
                lst =await this.Query<EmpProjectListViewModel>().FromSqlRaw(sql, para.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }
    }

}
