using System;
using System.Buffers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class PatronsController : Controller
  {
    private readonly LibraryContext _db;
    public PatronsController(LibraryContext db)
    {
      _db = db;
    }
    // INDEX ***********
    public ActionResult Index()
    {
      List<Patron> model = _db.Patrons.OrderBy(x => x.Name).ToList();
      return View(model);
    }
    // CREATE ************
    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Patron patron, int BookId)
    {
      _db.Patrons.Add(patron);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    // DETAILS ************
    public ActionResult Details(int id)
    {
      Patron model = _db.Patrons.FirstOrDefault(x => x.PatronId == id);
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
      foreach ( var book in books)
      {

      }
      ViewBag.Books = books;
      return View(model);
    }
    [HttpPost]
    public ActionResult AddBook(int BookId, int PatronId)
    {
      if (BookId != 0)
      {
        Copy copy = _db.Copies.FirstOrDefault(copies => copies.BookId == BookId && copies.OnShelf == true)
        _db.Checkouts.Add()
        _db.BookPatron.Add(new BookPatron() { BookId = BookId, PatronId = PatronId });
      }
      _db.SaveChanges();
      return RedirectToAction("Details", null, new { id = PatronId });
    }
  }
}