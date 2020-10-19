using System.Collections.Generic;

namespace Library.Models
{
    public class Patron
    {
        public Patron()
        {
            this.Books = new HashSet<BookPatron>();
        }

        public int PatronId { get; set; }
        public string Name { get; set; }
        public ICollection<BookPatron> Books { get; }
    }
}