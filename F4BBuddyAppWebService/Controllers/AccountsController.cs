using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using F4BBuddyAppWebService.Models;
using Internal;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Microsoft.Win32.SafeHandles;
using Microsoft.CodeAnalysis;

namespace F4BBuddyAppWebService.Controllers
{
    public class AccountsController : Controller
    {
        private readonly F4bContext _context;

        public AccountsController(F4bContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return _context.Accounts != null ?
                        View(await _context.Accounts.ToListAsync()) :
                        Problem("Entity set 'F4bContext.Accounts'  is null.");
        }

        // GET: AllStars/GetAllStars
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<ActionResult> Login(string? email, string? password)
        {
            if (email == null || password == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Email == email && m.Password == password);

            if (account == null)
            {
                return NotFound();
            }

            // Cookies Setting
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            // Claims contain subject's info
            var claims = new List<Claim>
            {
                new Claim("Id", "" + account.Id),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, "User"),
            };


            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok();
        }

        public async Task<IActionResult> LogOut()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }


        [HttpPost("Accounts/Register")]
        public async Task<IActionResult> Register(string name, int age, string gender, string school, string password, string email)
        {
            if (string.IsNullOrEmpty(name) || age <= 0 || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(school) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid input parameters.");
            }

            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (existingAccount != null)
            {
                return Conflict("Email already exists.");
            }

            var newAccount = new Account
            {
                Email = email,
                Password = password, // Note: Password should be hashed and salted before storing
                Money = 0
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            var newBuddy = new Buddy
            {
                AccountId = newAccount.Id,
                FirstName = name,
                Gender = gender,
                Age = age,
                School = school,
                SchoolYear = 1,
                GuardianEmail = email,
                IsGuardianConsented = "Yes"
            };

            _context.Buddies.Add(newBuddy);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Registration successful", AccountId = newAccount.Id, BuddyId = newBuddy.Id });
        }

        [Authorize]
        public async Task<IActionResult> GeneratePDF()
        {
            //string id = HttpContext.User.Claims.First(c => c.Type.Equals("Id")).Value;

            /*
             * PDF Generator go here
            */

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XImage image = XImage.FromFile(Environment.CurrentDirectory + "/test.png");

            page.Width = XUnit.FromMillimeter(image.PixelWidth * 25.4 / image.HorizontalResolution);
            page.Height = XUnit.FromMillimeter(image.PixelHeight * 25.4 / image.VerticalResolution);

            XFont fontRegular = new XFont("Times New Roman", 20, XFontStyle.Regular, XPdfFontOptions.UnicodeDefault);

            // Draw image
            gfx.DrawImage(image, 0, 0);

            // Write text at 1070:485 pixel position
            gfx.DrawString("My Name", fontRegular, XBrushes.Black, 1070.0 / image.PixelWidth * image.Size.Width, 485.0 / image.PixelHeight * image.Size.Height);
            gfx.DrawString("My Hobbies", fontRegular, XBrushes.Orange, 960.0 / image.PixelWidth * image.Size.Width, 600.0 / image.PixelHeight * image.Size.Height);
            gfx.DrawString("No reason", fontRegular, XBrushes.Cyan, 960.0 / image.PixelWidth * image.Size.Width, 775.0 / image.PixelHeight * image.Size.Height);

            // Send PDF to browser
            MemoryStream memoryStream = new MemoryStream();
            document.Save(memoryStream, false);

            //return new FileStreamResult(memoryStream, "application/pdf");
            return new FileStreamResult(memoryStream, "multipart/form-data");

            /*
             * PDF Generator go here
            */
        }

        
        public async Task<IActionResult> TestPDF()
        {
            //string id = HttpContext.User.Claims.First(c => c.Type.Equals("Id")).Value;

            /*
             * PDF Generator go here
            */

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XImage image = XImage.FromFile(Environment.CurrentDirectory + "/test.png");

            page.Width = XUnit.FromMillimeter(image.PixelWidth * 25.4 / image.HorizontalResolution);
            page.Height = XUnit.FromMillimeter(image.PixelHeight * 25.4 / image.VerticalResolution);

            XFont fontRegular = new XFont("Times New Roman", 20, XFontStyle.Regular, XPdfFontOptions.UnicodeDefault);

            // Draw image
            gfx.DrawImage(image, 0, 0);

            // Write text at 1070:485 pixel position
            gfx.DrawString("My Name", fontRegular, XBrushes.Black, 1070.0 / image.PixelWidth * image.Size.Width, 485.0 / image.PixelHeight * image.Size.Height);
            gfx.DrawString("My Hobbies", fontRegular, XBrushes.Orange, 960.0 / image.PixelWidth * image.Size.Width, 600.0 / image.PixelHeight * image.Size.Height);
            gfx.DrawString("No reason", fontRegular, XBrushes.Cyan, 960.0 / image.PixelWidth * image.Size.Width, 775.0 / image.PixelHeight * image.Size.Height);

            // Send PDF to browser
            MemoryStream memoryStream = new MemoryStream();
            document.Save(memoryStream, false);

            //return new FileStreamResult(memoryStream, "application/pdf");
            return new FileStreamResult(memoryStream, "multipart/form-data");

            /*
             * PDF Generator go here
            */
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,Money,Level1Completed,Level2Completed")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Money,Level1Completed,Level2Completed")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            return View(account);
        }


        //public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
        //{
        //    return await _context.Accounts.ToListAsync();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit([Bind("Id,Email,Password,Money,Level1Completed,Level2Completed")] Account account)
        {
            // Get id from cookie
            int id = int.Parse(HttpContext.User.Claims.First(c => c.Type.Equals("Id")).Value);

            // Get Account Detail
            if (_context.Accounts == null)
            {
                return NotFound();
            }

            account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }


            if (id != account.Id)
            {
                return NotFound();
            }
            //

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'F4bContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}