using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Precupanu_Alexia_Lab2.Models;
using Precupanu_Alexia_Lab2;

namespace Precupanu_Alexia_Lab2.Pages.BorrowingPages;

public class IndexModel : PageModel
{
    private readonly Precupanu_Alexia_Lab2Context _context;

    public IndexModel(Precupanu_Alexia_Lab2Context context)
    {
        _context = context;
    }

    public IList<Borrowing> Borrowing { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Borrowing != null)
        {
            Borrowing = await _context.Borrowing
                .Include(b => b.Book)
                    .ThenInclude(b => b.Authors)
                .Include(b => b.Member)
                .ToListAsync();
        }
    }
}
