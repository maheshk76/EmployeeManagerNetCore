using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManager.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required]
        public string FirstName  { get; set; }
        [Required]
        public string LastName  { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime JoiningDate { get; set; }
        [Required]
        public decimal Salary { get; set; }
    }
}
