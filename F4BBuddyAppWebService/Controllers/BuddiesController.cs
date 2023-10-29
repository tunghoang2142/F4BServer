using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using F4BBuddyAppWebService.Models;
using Microsoft.AspNetCore.Authorization;

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

        public async Task<ActionResult<Buddy>> GetDetails()
        {
            // Get account id from cookie
            int id = int.Parse(HttpContext.User.Claims.First(c => c.Type.Equals("Id")).Value);

            if (id == null || _context.Buddies == null)
            {
                return NotFound();
            }

            // Find buddy
            var buddy = await _context.Buddies.FirstOrDefaultAsync(m => m.AccountId == id);

            if (buddy == null)
            {
                return NotFound();
            }

            return buddy;
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
        public async Task<IActionResult> Create([Bind("Id,AccountId,FirstName,LastName,Gender,Age,School,SchoolYear,GuardianEmail,IsGuardianConsented,Hobbies,Motivation")] Buddy buddy)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,FirstName,LastName,Gender,Age,School,SchoolYear,GuardianEmail,IsGuardianConsented,Hobbies,Motivation")] Buddy buddy)
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Consumes("application/json")]
        [Authorize]
        public async Task<IActionResult> EditInfo([FromBody][Bind("FirstName,LastName,School,Hobbies,Motivation")] Buddy buddy)
        {
            // Get account id from cookie
            int id = int.Parse(HttpContext.User.Claims.First(c => c.Type.Equals("Id")).Value);

            // Get the current value
            var baseBuddy = await _context.Buddies.FirstOrDefaultAsync(m => m.AccountId == id);

            if (baseBuddy == null)
            {
                return NotFound();
            }

            // Update value
            baseBuddy.FirstName = buddy.FirstName;
            baseBuddy.LastName = buddy.LastName;
            baseBuddy.School = buddy.School;
            baseBuddy.Hobbies = buddy.Hobbies;
            baseBuddy.Motivation = buddy.Motivation;
            //buddy = baseBuddy;

            try
            {
                _context.Update(baseBuddy);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
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
