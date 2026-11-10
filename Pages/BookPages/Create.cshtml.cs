using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Precupanu_Alexia_Lab2.Models;
using Microsoft.AspNetCore.Authorization;

namespace Precupanu_Alexia_Lab2.Pages.BookPages;

[Authorize(Roles = "Admin")]
public class CreateModel : BookCategoriesPageModel
{
    private readonly Precupanu_Alexia_Lab2Context _context;

    public CreateModel(Precupanu_Alexia_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ViewData["PublisherID"] = new SelectList(
            _context.Publisher, "ID", "PublisherName");

        ViewData["AuthorsID"] = new SelectList(
            _context.Authors, "ID", "FirstName");

        Book = new Book
        {
            BookCategories = new List<BookCategory>()
        };

        PopulateAssignedCategoryData(_context, Book);

        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
    {
        var newBookCategories = new List<BookCategory>();

        if (selectedCategories != null)
        {
            foreach (var category in selectedCategories)
            {
                newBookCategories.Add(new BookCategory
                {
                    CategoryID = int.Parse(category)
                });
            }
        }

        Book.BookCategories = newBookCategories;

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

            PopulateAssignedCategoryData(_context, Book);
            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
