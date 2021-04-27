
using EmpManager.Models;
using EmpManager.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManager.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepository<Employee> _repo;
        public EmployeeController(IRepository<Employee> repository)
        {
            _repo = repository;
        }
        public IActionResult Index()
        {
         
            return View(_repo.Get());
        }
        public IActionResult EmpForm(int? id)
        {
            if (id == null)
            {
                ViewBag.VTitle = "Create";
                return View();
            }
            else
            {
                Employee emp = _repo.Get((int)id);
                ViewBag.VTitle = "Edit";
                return View(emp);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Employee employee)
        {
            if (employee.EmployeeId == 0)
                _repo.Insert(employee);
            else
                 _repo.Update(employee);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            _repo.Delete((int)id);
            JsonResult result = new JsonResult((Success: true, Message: "Deleted Successfully"));
            return result;
        }
    }
}
