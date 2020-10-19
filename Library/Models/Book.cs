using System.Collections.Generic;

namespace Library.Models
{
    public class Book
    {
        public Book()
        {
            this.Authors = new HashSet<AuthorBook>();
            this.Patrons = new HashSet<BookPatron>();
        }

        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser User { get; set; }
        public ICollection<AuthorBook> Authors { get; }
        public ICollection<BookPatron> Patrons { get; }
    }
}