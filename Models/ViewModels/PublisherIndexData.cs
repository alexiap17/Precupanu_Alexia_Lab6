using Precupanu_Alexia_Lab2.Models;

namespace Precupanu_Alexia_Lab2.ViewModels
{
    public class PublisherIndexData
    {
        public IEnumerable<Publisher> Publishers { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}