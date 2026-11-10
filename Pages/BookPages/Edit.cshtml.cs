
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Precupanu_Alexia_Lab2.Models;
using Microsoft.AspNetCore.Authorization;


namespace Precupanu_Alexia_Lab2.Pages.BookPages;

[Authorize(Roles = "Admin")]
public class EditModel : BookCategoriesPageModel
{
    private readonly Precupanu_Alexia_Lab2Context _context;

    public EditModel(Precupanu_Alexia_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.ID == id);

        if (book is null)
        {
            return NotFound();
        }

        Book = book;

        PopulateAssignedCategoryData(_context, Book);

        ViewData["PublisherID"] = new SelectList(
            _context.Publisher, "ID", "PublisherName", Book.PublisherID);

        ViewData["AuthorsID"] = new SelectList(
            _context.Authors, "ID", "FirstName", Book.AuthorsID);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
    {
        var bookToUpdate = await _context.Book
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.ID == Book.ID);

        if (bookToUpdate is null)
        {
            return NotFound();
        }

        if (Book.AuthorsID.HasValue)
        {
            var author = await _context.Authors.FindAsync(Book.AuthorsID.Value);
            Book.Author = author != null ? author.FirstName : string.Empty;
        }

        if (!ModelState.IsValid)
        {
            ViewData["PublisherID"] = new SelectList(
                _context.Publisher, "ID", "PublisherName", Book.PublisherID);

            ViewData["AuthorsID"] = new SelectList(
                _context.Authors, "ID", "FirstName", Book.AuthorsID);

            PopulateAssignedCategoryData(_context, bookToUpdate);
            return Page();
        }

        _context.Entry(bookToUpdate).CurrentValues.SetValues(Book);

        UpdateBookCategories(_context, selectedCategories, bookToUpdate);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}
