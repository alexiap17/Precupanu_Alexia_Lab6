using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Precupanu_Alexia_Lab2.Models;
using Precupanu_Alexia_Lab2;

namespace Precupanu_Alexia_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Precupanu_Alexia_Lab2Context _context;

    public IndexModel(Precupanu_Alexia_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = default!;

    public async Task OnGetAsync()
    {

        Category = await _context.Category
            .Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .ToListAsync();

    }
}
