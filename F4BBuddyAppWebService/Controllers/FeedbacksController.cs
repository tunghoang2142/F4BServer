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
    public class FeedbacksController : Controller
    {
        private readonly F4bContext _context;

        public FeedbacksController(F4bContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index()
        {
            var f4bContext = _context.Feedbacks.Include(f => f.AllStar).Include(f => f.Buddy);
            return View(await f4bContext.ToListAsync());
        }

        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        // GET: Feedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.AllStar)
                .Include(f => f.Buddy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            ViewData["AllStarId"] = new SelectList(_context.AllStars, "Id", "Id");
            ViewData["BuddyId"] = new SelectList(_context.Buddies, "Id", "Id");
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AllStarId,BuddyId,Feedback1")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AllStarId"] = new SelectList(_context.AllStars, "Id", "Id", feedback.AllStarId);
            ViewData["BuddyId"] = new SelectList(_context.Buddies, "Id", "Id", feedback.BuddyId);
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            ViewData["AllStarId"] = new SelectList(_context.AllStars, "Id", "Id", feedback.AllStarId);
            ViewData["BuddyId"] = new SelectList(_context.Buddies, "Id", "Id", feedback.BuddyId);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AllStarId,BuddyId,Feedback1")] Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
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
            ViewData["AllStarId"] = new SelectList(_context.AllStars, "Id", "Id", feedback.AllStarId);
            ViewData["BuddyId"] = new SelectList(_context.Buddies, "Id", "Id", feedback.BuddyId);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.AllStar)
                .Include(f => f.Buddy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Feedbacks == null)
            {
                return Problem("Entity set 'F4bContext.Feedbacks'  is null.");
            }
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
          return (_context.Feedbacks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
