
using System.ComponentModel.DataAnnotations;

namespace Precupanu_Alexia_Lab2.Models
{
    public class Authors
    {
        public int ID { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public ICollection<Book>? Books { get; set; }
    }
}
