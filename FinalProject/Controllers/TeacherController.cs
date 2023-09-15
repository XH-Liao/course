using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly TeacherContext _context;
        private readonly ClassesContext classesContext;

        public TeacherController(TeacherContext context,ClassesContext CContext)
        {
            _context = context;
            classesContext = CContext;
        }

        // GET: Teacher
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teacher.OrderBy(t=>t.Name).ToListAsync());
        }

        
        // GET: Teacher/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Teacher teacher = (from t in _context.Teacher
                                    where t.ID == id
                                    select t).SingleOrDefault();

            List<Classes> classes = (from c in classesContext.Classes
                                     where c.Teacher == teacher.Name
                                     orderby c.DayOfWeek ascending, c.Time ascending
                                     select c).ToList();

            if (teacher == null)
            {
                return NotFound();
            }
            if (classes.Count == 0)
            {
                ViewBag.NoClass = teacher.Name+" has no class!";
            }

            TeacherClass teacherClass = new TeacherClass();
            teacherClass.teacher = teacher;
            teacherClass.classes = classes;

            return View(teacherClass);
        }

        // GET: Teacher/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("ID,Name,Office,Phone,Email,Note")] Teacher teacher)
        {
            Teacher teachermodel = (from t in _context.Teacher
                                    where t.Name == teacher.Name
                                    select t).SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (teachermodel != null)
                {
                    ViewBag.Created = "<Error> " + teachermodel.Name + " has created !";
                    return View(teacher);
                }
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teacher/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Office,Phone,Email,Note")] Teacher teacher)
        {
            if (id != teacher.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Teacher teachermodel = (from t in _context.Teacher
                                        where t.ID != teacher.ID && t.Name == teacher.Name
                                        select t).SingleOrDefault();
                if (teachermodel != null)
                {
                    ViewBag.Created = "<Error> " + teachermodel.Name + " has created !";
                    return View(teacher);
                }
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teacher/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.ID == id);
        }
    }
}
