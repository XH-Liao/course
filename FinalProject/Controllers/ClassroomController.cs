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
    public class ClassroomController : Controller
    {
        private readonly ClassroomContext _context;
        private readonly ClassesContext classesContext;

        public ClassroomController(ClassroomContext context,ClassesContext CContext)
        {
            _context = context;
            classesContext = CContext;
        }

        // GET: Classroom
        public async Task<IActionResult> Index()
        {
            return View(await _context.Classroom.OrderBy(r=>r.Name).ToListAsync());
        }

        // GET: Classroom/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Classroom classroom = (from r in _context.Classroom
                                   where r.ID == id
                                   select r).SingleOrDefault();
            if (classroom == null)
            {
                return NotFound();
            }

            List<Classes> classes = (from c in classesContext.Classes
                                     where c.Room == classroom.Name
                                     orderby c.DayOfWeek ascending,c.Time ascending
                                     select c).ToList();
            if (classes.Count == 0)
            {
                ViewBag.NoClass = classroom.Name + " has no class!";
            }

            RoomClass roomClass = new RoomClass();
            roomClass.classroom = classroom;
            roomClass.classes = classes;

            return View(roomClass);
        }

        // GET: Classroom/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classroom/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("ID,Name,ToUse,Seat,Note")] Classroom classroom)
        {
            Classroom classroommodel = (from m in _context.Classroom
                                        where m.Name == classroom.Name
                                        select m).SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (classroommodel != null)
                {
                    ViewBag.Created = "<Error>  " + classroommodel.Name + " has created!";
                    return View(classroom);
                }
                _context.Add(classroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classroom);
        }

        // GET: Classroom/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classroom.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }
            return View(classroom);
        }

        // POST: Classroom/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,ToUse,Seat,Note")] Classroom classroom)
        {
            if (id != classroom.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Classroom classroommodel = (from m in _context.Classroom
                                            where m.ID!=classroom.ID && m.Name == classroom.Name
                                            select m).SingleOrDefault();
                if (classroommodel != null)
                {
                    ViewBag.Created = "<Error> " + classroommodel.Name + " has created !";
                    return View(classroom);
                }
                try
                {
                    _context.Update(classroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomExists(classroom.ID))
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
            return View(classroom);
        }

        // GET: Classroom/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classroom
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classroom == null)
            {
                return NotFound();
            }

            return View(classroom);
        }

        // POST: Classroom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroom = await _context.Classroom.FindAsync(id);
            _context.Classroom.Remove(classroom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomExists(int id)
        {
            return _context.Classroom.Any(e => e.ID == id);
        }
    }
}
