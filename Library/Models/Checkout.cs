using System;

namespace Library.Models
{
  public class Checkout
  {

    public int CheckoutId { get; set; }
    public int PatronId { get; set; }
    public int CopyId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public DateTime Due { get; set; }
    public virtual Patron Patron { get; set; }
    public virtual Copy Copy { get; set; }
  }
}