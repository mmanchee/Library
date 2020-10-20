using System;
using System.Buffers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Library.Controllers
{
  [Authorize]
  public class PatronsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public PatronsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      Console.WriteLine(userId);
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userPatrons = _db.Patrons.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(userPatrons);
    }
    // INDEX ***********
    // public ActionResult Index()
    // {
    //   List<Patron> model = _db.Patrons.OrderBy(x => x.Name).ToList();
    //   return View(model);
    // }
    // CREATE ************
    public ActionResult Create()
    {
      return View();
    }
    public async Task<ActionResult> Create(Patron patron, int BookId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      patron.User = currentUser;
      _db.Patrons.Add(patron);
      _db.SaveChanges();
      return RedirectToAction("Index");
      
    }
    // [HttpPost]
    // public ActionResult Create(Patron patron, int BookId)
    // {
    //   _db.Patrons.Add(patron);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }
    // DETAILS ************
    public ActionResult Details(int id)
    {
      List<object> Books = new List<object>();
      Patron model = _db.Patrons.FirstOrDefault(x => x.PatronId == id);
      List<Checkout> thisCopies = _db.Checkouts.Where(checkouts => checkouts.PatronId == id ).ToList();
      foreach(var copy in thisCopies)
      {
        Copy fart = _db.Copies.FirstOrDefault(copies => copies.CopyId == copy.CopyId);
        Book book = _db.Books.FirstOrDefault(books => books.BookId == fart.BookId);
        Books.Add(book);
      }
      ViewBag.Books = Books;
      return View(model);
    }
    // DELETE *************
    public ActionResult Delete(int id)
    {
      var thisPatron = _db.Patrons.FirstOrDefault(x => x.PatronId == id);
      return View(thisPatron);
    }
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisPatron = _db.Patrons.FirstOrDefault(x => x.PatronId == id);
      _db.Patrons.Remove(thisPatron);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    // EDIT **************
    public ActionResult Edit(int id)
    {
      var thisPatron = _db.Patrons.FirstOrDefault(x => x.PatronId == id);
      return View(thisPatron);
    }
    [HttpPost]
    public ActionResult Edit(Patron patron)
    {
      _db.Entry(patron).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    // ADD BOOK *************
    public ActionResult AddBook(int id)
    {
      Patron model = _db.Patrons.FirstOrDefault(patrons => patrons.PatronId == id);
      List<Book> books = _db.Books.OrderBy(x => x.Title).ToList();
      ViewBag.Books = books;
      return View(model);
    }
    [HttpPost]
    public ActionResult AddBook(int BookId, int PatronId)
    {
      Copy copy = _db.Copies.FirstOrDefault(copies => copies.BookId == BookId && copies.OnShelf == true);
      if (copy.CopyId > 0)
      {
        DateTime today = DateTime.Now;
        copy.OnShelf = false;
        _db.Checkouts.Add(new Checkout() { CopyId = copy.CopyId, PatronId = PatronId, CheckOut = today, Due = today.AddDays(5)});
        _db.SaveChanges();
        return RedirectToAction("Details", null, new { id = PatronId });
      }
      else
      {
        ViewBag.NotHere = "Book not available.";
        return RedirectToAction("AddBook", null, new { id = PatronId });
      }
    }
  }
}
