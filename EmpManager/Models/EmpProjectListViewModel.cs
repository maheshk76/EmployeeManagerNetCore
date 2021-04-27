using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManager.Models
{
    public class EmpProjectListViewModel
    {
        public int EmpToProjectId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string FullName { get; set; }
        public string ProjectName { get; set; }
        public decimal Cost { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
