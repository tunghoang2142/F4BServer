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
    public class BuddiesController : Controller
    {
        private readonly F4bContext _context;

        public BuddiesController(F4bContext context)
        {
            _context = context;
        }

        // GET: Buddies
        public async Task<IActionResult> Index()
        {
            var f4bContext = _context.Buddies.Include(b => b.Account);
            return View(await f4bContext.ToListAsync());
        }

        public async Task<ActionResult<IEnumerable<Buddy>>> GetAllBuddies()
        {
            return await _context.Buddies.ToListAsync();
        }

        // GET: Buddies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Buddies == null)
            {
                return NotFound();
            }

            var buddy = await _context.Buddies
                .Include(b => b.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buddy == null)
            {
                return NotFound();
            }

            return View(buddy);
        }

        // GET: Buddies/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id");
            return View();
        }

        // POST: Buddies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,FirstName,LastName,Gender,Age,School,SchoolYear,GuardianEmail,IsGuardianConsented")] Buddy buddy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buddy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", buddy.AccountId);
            return View(buddy);
        }

        // GET: Buddies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Buddies == null)
            {
                return NotFound();
            }

            var buddy = await _context.Buddies.FindAsync(id);
            if (buddy == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", buddy.AccountId);
            return View(buddy);
        }

        // POST: Buddies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,FirstName,LastName,Gender,Age,School,SchoolYear,GuardianEmail,IsGuardianConsented")] Buddy buddy)
        {
            if (id != buddy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buddy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuddyExists(buddy.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", buddy.AccountId);
            return View(buddy);
        }

        // GET: Buddies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Buddies == null)
            {
                return NotFound();
            }

            var buddy = await _context.Buddies
                .Include(b => b.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buddy == null)
            {
                return NotFound();
            }

            return View(buddy);
        }

        // POST: Buddies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Buddies == null)
            {
                return Problem("Entity set 'F4bContext.Buddies'  is null.");
            }
            var buddy = await _context.Buddies.FindAsync(id);
            if (buddy != null)
            {
                _context.Buddies.Remove(buddy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuddyExists(int id)
        {
          return (_context.Buddies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
