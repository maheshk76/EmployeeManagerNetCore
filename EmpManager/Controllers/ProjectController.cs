using EmpManager.Constants;
using EmpManager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManager.Controllers
{
    public class ProjectController : Controller
    {
        private readonly EmpDbContext _context;
        public ProjectController(EmpDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Projects.ToList());
        }
        public IActionResult ProjectForm(int? id)
        {
            if (id == null)
            {
                ViewBag.VTitle = ViewType.Create;
                return View();
            }
            else
            {
                Project emp = _context.Projects.Find(id);
                ViewBag.VTitle = ViewType.Edit;
                return View(emp);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Project project)
        {
            if (project.ProjectId == 0)
                _context.Projects.Add(project);
            else
                _context.Update(project);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<JsonResult> Delete(int? id)
        {
            var empInDb = await _context.Projects.FindAsync(id);

            _context.Projects.Remove(empInDb);
            await _context.SaveChangesAsync();
            JsonResult result = new JsonResult(new { Success = true, Message = ResponseMessage.Deleted });
            return result;
        }
    }
}
