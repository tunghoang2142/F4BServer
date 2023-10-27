using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using F4BBuddyAppWebService.Models;

namespace F4BBuddyAppWebService.Views.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly F4BBuddyAppWebService.Models.F4bContext _context;

        public IndexModel(F4BBuddyAppWebService.Models.F4bContext context)
        {
            _context = context;
        }

        public IList<Account> Account { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Accounts != null)
            {
                Account = await _context.Accounts.ToListAsync();
            }
        }
    }
}
