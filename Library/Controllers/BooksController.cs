using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    public BooksController(LibraryContext db)
    {
      _db = db;
    }
    public ActionResult Index()
    {
      List<Book> model = _db.Books.OrderBy(x => x.Title).ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View();
    }
    [HttpPost]
    public ActionResult Create(Book book, int AuthorId)
    {
      _db.Books.Add(book);
      if (AuthorId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { BookId = book.BookId, AuthorId = AuthorId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      Book model = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(model);
    }
    public ActionResult Delete(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(x => x.BookId == id);
      return View(thisBook);
    }
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(x => x.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult DeleteAuthor(int joinId, int BookId)
    {
      var joinEntry = _db.AuthorBook.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBook.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = BookId });
    }
    public ActionResult Edit(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(Books => Books.BookId == id);
      var thisAB = _db.AuthorBook.FirstOrDefault(ab => ab.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name", thisAB.AuthorId);
      return View(thisBook);
    }
    [HttpPost]
    public ActionResult Edit(Book book, int AuthorId)
    {
      if (AuthorId != 0)
      {
        bool tf = _db.AuthorBook.Any(x => x.AuthorId == AuthorId && x.BookId == book.BookId);
        if (!tf)
        {
          _db.AuthorBook.Add(new AuthorBook() { BookId = book.BookId, AuthorId = AuthorId });
        }
      }
      _db.Entry(book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
//make a add copy fuction 