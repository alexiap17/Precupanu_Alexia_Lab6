
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Precupanu_Alexia_Lab2.Models;

namespace Precupanu_Alexia_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Precupanu_Alexia_Lab2Context _context;

    public IndexModel(Precupanu_Alexia_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = new List<Book>();

    public BookData BookD { get; set; } = new BookData();

    public int BookID { get; set; }

    public int CategoryID { get; set; }

    public string TitleSort { get; set; }
    public string AuthorSort { get; set; }

    public string CurrentFilter { get; set; }

    public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString )
    {
        BookD = new BookData();
        TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        AuthorSort = sortOrder == "author" ? "author_desc" : "author";

        CurrentFilter = searchString;

        BookD.Books = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

        if (!String.IsNullOrEmpty(searchString))
        {
            BookD.Books = BookD.Books.Where(s =>
                s.Authors!.FirstName.Contains(searchString) ||
                s.Authors.LastName.Contains(searchString) ||
                s.Title.Contains(searchString)).ToList();
        }
        Book = BookD.Books.ToList();
        if (id != null)
        {
            BookID = id.Value;

            var book = BookD.Books
                .SingleOrDefault(b => b.ID == id.Value);

            if (book != null)
            {
                BookD.Categories = book.BookCategories?
                    .Select(bc => bc.Category)
                    .Where(c => c != null)
                    .ToList() ?? new List<Category>();
            }
        }
        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Title).ToList();
                break;

            case "author_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Authors!.FullName).ToList();
                break;

            case "author":
                BookD.Books = BookD.Books.OrderBy(s => s.Authors!.FullName).ToList();
                break;

            default:
                BookD.Books = BookD.Books.OrderBy(s => s.Title).ToList();
                break;
        }
        if (categoryID != null)
        {
            CategoryID = categoryID.Value;
        }
    }
}
