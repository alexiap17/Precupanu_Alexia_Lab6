using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Precupanu_Alexia_Lab2.Models;
using Precupanu_Alexia_Lab2;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Precupanu_Alexia_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Precupanu_Alexia_Lab2Context _context;

    public EditModel(Precupanu_Alexia_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);

        if (borrowing is null)
        {
            return NotFound();
        }

        Borrowing = borrowing;

        await LoadSelectListsAsync();

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
   
    private async Task LoadSelectListsAsync()
    {
        var members = await _context.Member.ToListAsync();

        ViewData["MemberID"] = new SelectList(
            members, "ID", "FullName", Borrowing.MemberID);

        var books = await _context.Book
            .Include(b => b.Authors)
            .ToListAsync();

        ViewData["BookID"] = new SelectList(
            books.Select(b => new
            {
                b.ID,
                Display = b.Title + " - " + b.Authors!.FullName
            }),
            "ID",
            "Display",
            Borrowing.BookID);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectListsAsync();
            return Page();
        }

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BorrowingExists(Borrowing.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool BorrowingExists(int id)
    {
        return _context.Borrowing.Any(e => e.ID == id);
    }
}
