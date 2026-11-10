using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Precupanu_Alexia_Lab2.Models;

namespace Precupanu_Alexia_Lab2.Pages.CategoryPages;

public class DetailsModel : PageModel
{
    private readonly Precupanu_Alexia_Lab2Context _context;

    public DetailsModel(Precupanu_Alexia_Lab2Context context)
    {
        _context = context;
    }

    public Category Category { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Category
    .Include(c => c.BookCategories)
        .ThenInclude(bc => bc.Book)
            .ThenInclude(b => b.Authors)
    .FirstOrDefaultAsync(c => c.ID == id);

        if (category == null)
        {
            return NotFound();
        }

        Category = category;

        return Page();
    }
}