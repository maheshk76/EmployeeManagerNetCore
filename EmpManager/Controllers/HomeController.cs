using EmpManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmpDbContext _context;
        private List<Employee> employees;
        private List<Project> projects;
        public HomeController(ILogger<HomeController> logger, EmpDbContext context)
        {
            _logger = logger;
            _context = context;
             employees= _context.Employees.ToList();
             employees.Insert(0, new Employee { EmployeeId = 0, FirstName = "--Select Employee--" });
             projects= _context.Projects.ToList();
             projects.Insert(0, new Project { ProjectId = 0, Name = "--Assign Project--" });
        }
       
        public IActionResult AssignProject(int? id)
        {
            EmployeeToProject model = new EmployeeToProject();
            
            if (id != null)
            {
                model = _context.EmployeeToProjects.Find(id);
            }
            model.EmpList = employees;
            model.ProjectList = projects;
            return View(model);
             
        }
        [HttpPost]
        public async Task<IActionResult> AssignProject(EmployeeToProject employeeToProject)
        {
            EmployeeToProject DataInDb = _context.EmployeeToProjects.FirstOrDefault(x=>x.EmployeeId==employeeToProject.EmployeeId && x.ProjectId==employeeToProject.ProjectId);
            if (DataInDb != null)
            {
                employeeToProject.EmpList = employees;
                employeeToProject.ProjectList = projects;
                ModelState.AddModelError("ModelOnly",
                                         "This Project has already assigned to " + DataInDb.Employee.FirstName);
                return View(employeeToProject);
            }
            if (employeeToProject.Id == 0)
            {
                _context.EmployeeToProjects.Add(employeeToProject);
            }
            else
            {
                _context.Update(employeeToProject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View(new FilterModel());
        }
        public async Task<ActionResult> MainList(string search, string sdate, string edate, int cPage = 1, int Pagesize = 5)
        {
            search ??= "";
            sdate ??= "";
            edate ??= "";
            List<object> parameters = new List<object>() {
                new SqlParameter("@search", search),
                new SqlParameter("@sdate", sdate),
                new SqlParameter("@edate", edate),
                new SqlParameter("@pagesize", Pagesize),
                new SqlParameter("@pagenum", cPage)
            };
            SqlParameter outparam = new SqlParameter
            {
                ParameterName = "possiblerows",
                DbType = System.Data.DbType.String,
                Size = Int32.MaxValue,
                Direction = System.Data.ParameterDirection.Output
            };
            parameters.Add(outparam);
            string query = "exec GetData @search,@sdate,@edate,@pagesize,@pagenum,@possiblerows OUTPUT";
            List<EmpProjectListViewModel> Employeelist = await _context.GetData(query, parameters);

            int possiblerows = Convert.ToInt32(outparam.Value);
            if (possiblerows > Pagesize)
            {
                var TotalPages = (int)Math.Ceiling((double)((decimal)possiblerows / Pagesize));
                ViewBag.TotalPages = TotalPages;
                ViewBag.CurrPage = cPage;
            }
            return PartialView(Employeelist);
        }
        [Route("api/search")]
        public  IActionResult Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names =   _context.Employees.Where(p => (p.FirstName+" "+p.LastName).StartsWith(term)).Select(p=> (p.FirstName + " " + p.LastName+"|")).ToList();
                var projects =  _context.Projects.Where(p => p.Name.StartsWith(term)).Select(p=> p.Name+"|Project").ToList();
                names.AddRange(projects);
                names.Sort();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<JsonResult> Delete(int? id)
        {
            EmployeeToProject empInDb = await _context.EmployeeToProjects.FindAsync(id);
            _context.EmployeeToProjects.Remove(empInDb);
            await _context.SaveChangesAsync();
            JsonResult result = new JsonResult((Success: true, Message: "Deleted Successfully"));
            return result;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
