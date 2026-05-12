using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorLog.Data;
using VisitorLog.Models;

namespace VisitorLog.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private const string SessionKey = "IsAuthenticated";

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString(SessionKey) == "true";
        }

        // GET: Home/Index
        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            if (!IsAuthenticated()) return RedirectToAction("Login", "Account");

            ViewBag.Search = search;

            var visitors = _context.Visitors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                visitors = visitors.Where(v =>
                    v.FullName.ToLower().Contains(search) ||
                    v.PurposeOfVisit.ToLower().Contains(search) ||
                    v.PersonToVisit.ToLower().Contains(search) ||
                    v.ContactNumber.Contains(search));
            }

            var list = await visitors.OrderByDescending(v => v.DateTimeVisited).ToListAsync();
            return View(list);
        }

        // POST: Add Visitor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Visitor visitor)
        {
            if (!IsAuthenticated()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                visitor.DateTimeVisited = DateTime.Now;
                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Visitor <strong>{visitor.FullName}</strong> has been added successfully.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to add visitor. Please check all fields.";
            var list = await _context.Visitors.OrderByDescending(v => v.DateTimeVisited).ToListAsync();
            return View("Index", list);
        }

        // GET: Get visitor JSON for edit modal
        [HttpGet]
        public async Task<IActionResult> GetVisitor(int id)
        {
            if (!IsAuthenticated()) return Unauthorized();

            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null) return NotFound();

            return Json(new
            {
                visitor.Id,
                visitor.FullName,
                visitor.PurposeOfVisit,
                visitor.PersonToVisit,
                visitor.ContactNumber,
                DateTimeVisited = visitor.DateTimeVisited.ToString("yyyy-MM-ddTHH:mm")
            });
        }

        // POST: Edit Visitor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Visitor visitor)
        {
            if (!IsAuthenticated()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                var existing = await _context.Visitors.FindAsync(visitor.Id);
                if (existing == null)
                {
                    TempData["Error"] = "Visitor not found.";
                    return RedirectToAction("Index");
                }

                existing.FullName = visitor.FullName;
                existing.PurposeOfVisit = visitor.PurposeOfVisit;
                existing.PersonToVisit = visitor.PersonToVisit;
                existing.ContactNumber = visitor.ContactNumber;
                existing.DateTimeVisited = visitor.DateTimeVisited;

                await _context.SaveChangesAsync();
                TempData["Success"] = $"Visitor <strong>{visitor.FullName}</strong> has been updated successfully.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to update visitor. Please check all fields.";
            var list = await _context.Visitors.OrderByDescending(v => v.DateTimeVisited).ToListAsync();
            return View("Index", list);
        }

        // POST: Delete Visitor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAuthenticated()) return RedirectToAction("Login", "Account");

            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor != null)
            {
                _context.Visitors.Remove(visitor);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Visitor <strong>{visitor.FullName}</strong> has been deleted.";
            }
            else
            {
                TempData["Error"] = "Visitor not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
