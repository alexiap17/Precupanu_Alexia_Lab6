using Microsoft.EntityFrameworkCore;

namespace Precupanu_Alexia_Lab2
{
    public class Precupanu_Alexia_Lab2Context : DbContext
    {
        public Precupanu_Alexia_Lab2Context(
            DbContextOptions<Precupanu_Alexia_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Precupanu_Alexia_Lab2.Models.Book> Book { get; set; } = default!;

        public DbSet<Precupanu_Alexia_Lab2.Models.Publisher> Publisher { get; set; } = default!;

        public DbSet<Precupanu_Alexia_Lab2.Models.Authors> Authors { get; set; } = default!;

        public DbSet<Precupanu_Alexia_Lab2.Models.Category> Category { get; set; } = default!;

        public DbSet<Precupanu_Alexia_Lab2.Models.BookCategory> BookCategory { get; set; } = default!;

        public DbSet<Precupanu_Alexia_Lab2.Models.Member> Member { get; set; } = default!;

        public DbSet<Precupanu_Alexia_Lab2.Models.Borrowing> Borrowing { get; set; } = default!;

    }
}