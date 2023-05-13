using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;

namespace SchoolManagementSystem.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SchoolDbContext _context;

        public SubjectsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            var subjects = await _context.Subjects.Include(s => s.Teacher).Include(s => s.Class).ToListAsync();
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name");
            return View(subjects);
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Class)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            return View();
        }

        // POST: Subjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TeacherId,ClassId")] Subject subject)
        {
            subject.Teacher = _context.Teachers.First(x => x.Id == subject.TeacherId);
            subject.Class = _context.Classes.First(x => x.Id == subject.ClassId);

            ModelState.Remove("Teacher");
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", subject.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", subject.TeacherId);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            var viewModel = new SubjectEditViewModel
            {
                Id = subject.Id,
                Name = subject.Name,
                TeacherId = subject.TeacherId,
                ClassId = subject.ClassId
            };

            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", viewModel.TeacherId);
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", viewModel.ClassId);

            return View(viewModel);
        }

        // POST: Subjects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubjectEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Teachers");
            ModelState.Remove("Classes");


            if (ModelState.IsValid)
            {
                try
                {
                    var subject = await _context.Subjects.FindAsync(viewModel.Id);
                    if (subject == null)
                    {
                        return NotFound();
                    }

                    subject.Name = viewModel.Name;
                    subject.TeacherId = viewModel.TeacherId;
                    subject.ClassId = viewModel.ClassId;

                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(viewModel.Id))
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

            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", viewModel.TeacherId);
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", viewModel.ClassId);

            return View(viewModel);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Class)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}