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
      _db=db;
    }
    public ActionResult Index()
    {
      List<Book> model = _db.Books.OrderBy(x=>x.Title).ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Book book)
    {
      _db.Books.Add(book);
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
    public ActionResult Edit(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(Books => Books.BookId == id);
      return View(thisBook);
    }
    [HttpPost]
    public ActionResult Edit(Book Book)
    {
      _db.Entry(Book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
//make a add copy fuction 