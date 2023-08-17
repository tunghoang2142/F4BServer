using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using F4BBuddyAppWebService.Models;

namespace F4BBuddyAppWebService.Controllers
{
    public class AllStarsController : Controller
    {
        private readonly F4bContext _context;

        public AllStarsController(F4bContext context)
        {
            _context = context;
        }

        // GET: AllStars
        public async Task<IActionResult> Index()
        {
              return _context.AllStars != null ? 
                          View(await _context.AllStars.ToListAsync()) :
                          Problem("Entity set 'F4bContext.AllStars'  is null.");
        }

        // GET: AllStars/GetAllStars
        public async Task<ActionResult<IEnumerable<AllStar>>> GetAllStars()
        {
            return await _context.AllStars.ToListAsync();
        }

        // GET: AllStars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AllStars == null)
            {
                return NotFound();
            }

            var allStar = await _context.AllStars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allStar == null)
            {
                return NotFound();
            }

            return View(allStar);
        }

        // GET: AllStars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AllStars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Gender,Age,School")] AllStar allStar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allStar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(allStar);
        }

        // GET: AllStars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AllStars == null)
            {
                return NotFound();
            }

            var allStar = await _context.AllStars.FindAsync(id);
            if (allStar == null)
            {
                return NotFound();
            }
            return View(allStar);
        }

        // POST: AllStars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Gender,Age,School")] AllStar allStar)
        {
            if (id != allStar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allStar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllStarExists(allStar.Id))
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
            return View(allStar);
        }

        // GET: AllStars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AllStars == null)
            {
                return NotFound();
            }

            var allStar = await _context.AllStars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allStar == null)
            {
                return NotFound();
            }

            return View(allStar);
        }

        // POST: AllStars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AllStars == null)
            {
                return Problem("Entity set 'F4bContext.AllStars'  is null.");
            }
            var allStar = await _context.AllStars.FindAsync(id);
            if (allStar != null)
            {
                _context.AllStars.Remove(allStar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllStarExists(int id)
        {
          return (_context.AllStars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
