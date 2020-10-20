using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class AuthorsController : Controller
  {
    private readonly LibraryContext _db;
    public AuthorsController(LibraryContext db)
    {
      _db = db;
    }
    public ActionResult Index()
    {
      List<Author> model = _db.Authors.OrderBy(x => x.Name).ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View();
    }
    [HttpPost]
    public ActionResult Create(Author author, int BookId)
    {
      _db.Authors.Add(author);
      if (BookId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { BookId = BookId, AuthorId = author.AuthorId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    // public ActionResult AddBook(int id)
    // {
    //   Book thisBook = _db.Books.FirstOrDefault(s => s.BookId == id);
    //   ViewBag.BookId = new SelectList(_db.Books, "BookId", "Name");
    //   return View(thisBook);
    // }
    // [HttpPost]
    // public ActionResult AddBook(AuthorBook AuthorBook)
    // {
    //   if (AuthorBook.BookId != 0)
    //   {
    //     if (_db.AuthorBook.Where(x => x.BookId == AuthorBook.BookId && x.BookId == AuthorBook.BookId).ToHashSet().Count == 0)
    //     {
    //       _db.AuthorBook.Add(AuthorBook);
    //     }
    //   }
    // }
    public ActionResult Details(int id)
    {
      Author model = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      return View(model);
    }
    public ActionResult Delete(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(x => x.AuthorId == id);
      return View(thisAuthor);
    }
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(x => x.AuthorId == id);
      _db.Authors.Remove(thisAuthor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult DeleteBook(int joinId, int AuthorId)
    {
      var joinEntry = _db.AuthorBook.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBook.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = AuthorId });
    }
    public ActionResult Edit(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(Authors => Authors.AuthorId == id);
      var thisAB = _db.AuthorBook.FirstOrDefault(ab => ab.AuthorId == thisAuthor.AuthorId);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title", thisAB.BookId);
      return View(thisAuthor);
    }
    [HttpPost]
    public ActionResult Edit(Author author, int BookId)
    {
      if (BookId != 0)
      {
        bool tf = _db.AuthorBook.Any(x => x.AuthorId == author.AuthorId && x.BookId == BookId);
        if (!tf)
        {
          _db.AuthorBook.Add(new AuthorBook() { BookId = BookId, AuthorId = author.AuthorId });
        }
      }
      _db.Entry(author).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}