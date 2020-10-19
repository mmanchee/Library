using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
  public class LibraryContext : DbContext
  {
    // public DbSet<Doctor> Doctors { get; set; }
    public LibraryContext(DbContextOptions options) : base(options) { }
  }
}