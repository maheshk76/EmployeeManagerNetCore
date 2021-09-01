
using EmpManager.Constants;
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
            List<Employee> Employees = _repo.Get();
            return View(Employees);
        }
        public IActionResult EmpForm(int? id)
        {
            if (id == null)
            {
                ViewBag.VTitle = ViewType.Create;
                return View();
            }
            else
            {
                Employee employee = _repo.Get((int)id);
                ViewBag.VTitle = ViewType.Edit;
                return View(employee);
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
            JsonResult result = new JsonResult((Success: true, Message: ResponseMessage.Deleted));
            return result;
        }
    }
}
