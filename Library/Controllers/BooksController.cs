using System;
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
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    public ActionResult Index()
    {
      List<Book> model = _db.Books.OrderBy(x => x.Title).ToList();
      return View(model);
    }
    // public async Task<ActionResult> Index()
    // {
    //   var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   var currentUser = await _userManager.FindByIdAsync(userId);
    //   var userBooks = _db.Books.Where(entry => entry.User.Id == currentUser.Id).ToList();
    //   return View(userBooks);
    // }
    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return
      View();
    }
    // [HttpPost]
    // public async Task<ActionResult> Create(Book book, int AuthorId)
    // {
    //   var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   var currentUser = await _userManager.FindByIdAsync(userId);
    //   book.User = currentUser;
    //   _db.Books.Add(book);
    //   if (AuthorId != 0)
    //   {
    //     _db.AuthorBook.Add(new AuthorBook() { BookId = book.BookId, AuthorId = AuthorId });
    //   }
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }
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
      List<Copy> copy = _db.Copies.Where(copies => copies.BookId == id).ToList();
      ViewBag.CopyCount = copy.Count;
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
      if(thisAB != null)
      {
        ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name", thisAB.AuthorId);
      }
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
    public ActionResult AddCopy(int id)
    {
      _db.Copies.Add( new Copy() { BookId = id, OnShelf = true } );
      _db.SaveChanges();
      return RedirectToAction("Details", null, new { id = id });
    }
    public ActionResult DeleteCopy(int id)
    {
      var copy = _db.Copies.FirstOrDefault(copies=>copies.BookId == id && copies.OnShelf == true);
      if (copy != null )
      {
        _db.Copies.Remove(copy);
        _db.SaveChanges();
      }
      return RedirectToAction("Details", null, new { id = id });
    }
  }
}

//make a add copy fuction 