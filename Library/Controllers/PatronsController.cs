using System;
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
      _db=db;
    }
    public ActionResult Index()
    {
      List<Patron> model = _db.Patrons.OrderBy(x=>x.Name).ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Patron patron)
    {
      _db.Patrons.Add(patron);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
     public ActionResult Details(int id)
    {
      Patron model = _db.Patrons.FirstOrDefault(x => x.PatronId == id);
      return View(model);
    }
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
  }
}