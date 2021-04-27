using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManager.Models
{
    public class EmployeeToProject
    {
        public int Id { get; set; }
        [Display(Name ="Employee")]
        public int EmployeeId { get; set; }
        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        public Employee Employee { get; set; }
        public Project Project { get; set; }
        [NotMapped]
        public List<Employee> EmpList { get; set; }
        [NotMapped]
        public List<Project> ProjectList { get; set; }
    }
}
