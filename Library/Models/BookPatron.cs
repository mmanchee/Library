using System;
using System.Globalization;

namespace Library.Models
{
  public class BookPatron
  {       
    public int BookPatronId { get; set; }
    public int BookId { get; set; }
    public int PatronId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public DateTime Due { get; set; }
    public int CopyId { get; set; }
    public virtual Book Book { get; set; }
    public virtual Patron Patron { get; set; }
  }
}