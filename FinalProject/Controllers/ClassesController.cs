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
    public class ClassesController : Controller
    {
        private readonly ClassesContext _context;
        private readonly TeacherContext teacherContext;
        private readonly ClassroomContext classroomContext;

        public ClassesController(ClassesContext context,TeacherContext Tcontext,ClassroomContext Rcontext)
        {
            _context = context;
            teacherContext = Tcontext;
            classroomContext = Rcontext;
        }

        private void FillTeacher()
        {
            List<SelectListItem> teachers = (from t in teacherContext.Teacher
                                      orderby t.Name ascending
                                      select new SelectListItem()
                                      {
                                          Text=t.Name,
                                          Value=t.Name
                                      }).ToList();
            ViewBag.Teacher = teachers;
        }

        private void FillClassroom()
        {
            List<SelectListItem> classrooms = (from r in classroomContext.Classroom
                                          orderby r.Name ascending
                                          select new SelectListItem()
                                          {
                                              Text=r.Name,
                                              Value=r.Name
                                          }).ToList();
            ViewBag.Classroom = classrooms;
        }


        private void FillDayOfWeek()
        {
            List<SelectListItem> dayOfWeek = new List<SelectListItem>
            {
                new SelectListItem{Text="Monday",Value="1"},
                new SelectListItem{Text="Tuesday",Value="2"},
                new SelectListItem{Text="Wednesday",Value="3"},
                new SelectListItem{Text="Thursday",Value="4"},
                new SelectListItem{Text="Friday",Value="5"}
            };
            ViewBag.DayOfWeek = dayOfWeek;
        }
        private void FillTime()
        {
            List<SelectListItem> times = new List<SelectListItem>
            {
                new SelectListItem{Text="第一節",Value="1"},
                new SelectListItem{Text="第二節",Value="2"},
                new SelectListItem{Text="第三節",Value="3"},
                new SelectListItem{Text="第四節",Value="4"},
                new SelectListItem{Text="第五節",Value="5"},
                new SelectListItem{Text="第六節",Value="6"},
                new SelectListItem{Text="第七節",Value="7"},
                new SelectListItem{Text="第八節",Value="8"},
                new SelectListItem{Text="第九節",Value="9"},
                new SelectListItem{Text="第十節",Value="10"},
                new SelectListItem{Text="第十一節",Value="11"},
                new SelectListItem{Text="第十二節",Value="12"},
                new SelectListItem{Text="第十三節",Value="13"}
            };
            ViewBag.Time = times;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Classes.OrderBy(c=>c.DayOfWeek).ThenBy(c=>c.Time).ToListAsync());
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        [Authorize(Roles = "Manager")]
        // GET: Classes/Create
        public IActionResult Create()
        {
            FillTeacher();
            FillClassroom();
            FillDayOfWeek();
            FillTime();
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("ID,ClassName,CourseCredit,Teacher,Room,DayOfWeek,Time,Note")] Classes classes)
        {
            FillTeacher();
            FillClassroom();
            FillDayOfWeek();
            FillTime();

            if (ModelState.IsValid)
            {
                List<Classes> TRT = (from t in _context.Classes
                                     where t.Teacher == classes.Teacher && t.Room == classes.Room
                                     && t.DayOfWeek == classes.DayOfWeek && t.Time == classes.Time
                                     select t).ToList();
                if (TRT.Count != 0)
                {
                    ViewBag.ErrorMessage = "<重複排課> 請確認後再排課！";
                    return View(classes);
                }

                List<Classes> TT = (from t in _context.Classes
                                    where t.Teacher == classes.Teacher && t.DayOfWeek == classes.DayOfWeek && t.Time == classes.Time
                                    select t).ToList();
                if (TT.Count != 0)
                {
                    ViewBag.ErrorMessage = "<老師>衝堂錯誤，請確認後再排課！";
                    return View(classes);
                }

                List<Classes> RT = (from r in _context.Classes
                                    where r.Room == classes.Room && r.DayOfWeek == classes.DayOfWeek && r.Time == classes.Time
                                    select r).ToList();
                if (RT.Count != 0)
                {
                    ViewBag.ErrorMessage = "<教室>衝堂錯誤，請確認後再排課！";
                    return View(classes);
                }
                _context.Add(classes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classes);
        }

        // GET: Classes/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FillTeacher();
            FillClassroom();
            FillDayOfWeek();
            FillTime();

            var classes = await _context.Classes.FindAsync(id);
            if (classes == null)
            {
                return NotFound();
            }
            return View(classes);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClassName,CourseCredit,Teacher,Room,DayOfWeek,Time,Note")] Classes classes)
        {
            if (id != classes.ID)
            {
                return NotFound();
            }

            FillTeacher();
            FillClassroom();
            FillDayOfWeek();
            FillTime();

            if (ModelState.IsValid)
            {
                List<Classes> TRT = (from t in _context.Classes
                                     where t.ID!=classes.ID && t.Teacher == classes.Teacher && t.Room == classes.Room 
                                     && t.DayOfWeek == classes.DayOfWeek && t.Time == classes.Time
                                     select t).ToList();
                if (TRT.Count != 0)
                {
                    ViewBag.ErrorMessage = "<重複排課> 請確認後再排課！";
                    return View(classes);
                }

                List<Classes> TT = (from t in _context.Classes
                                    where t.ID != classes.ID && t.Teacher == classes.Teacher && t.DayOfWeek == classes.DayOfWeek && t.Time == classes.Time
                                    select t).ToList();
                if (TT.Count != 0)
                {
                    ViewBag.ErrorMessage = "<老師>衝堂錯誤，請確認後再排課！";
                    return View(classes);
                }

                List<Classes> RT = (from r in _context.Classes
                                    where r.ID != classes.ID && r.Room == classes.Room && r.DayOfWeek == classes.DayOfWeek && r.Time == classes.Time
                                    select r).ToList();
                if (RT.Count != 0)
                {
                    ViewBag.ErrorMessage = "<教室>衝堂錯誤，請確認後再排課！";
                    return View(classes);
                }

                try
                {
                    _context.Update(classes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassesExists(classes.ID))
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
            return View(classes);
        }

        // GET: Classes/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classes = await _context.Classes.FindAsync(id);
            _context.Classes.Remove(classes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassesExists(int id)
        {
            return _context.Classes.Any(e => e.ID == id);
        }
    }
}
