using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class CopiesController : Controller
  {
    private readonly LibraryContext _db;
    public CopiesController(LibraryContext db)
    {
      _db=db;
    }
    public ActionResult Index()
    {
      List<Copy> model = _db.Copies.OrderBy(x=>x.CopyId).ToList();
      return View(model);
    }
    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Copy copy)
    {
      _db.Copies.Add(copy);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
     public ActionResult Details(int id)
    {
      Copy model = _db.Copies.FirstOrDefault(copy => copy.CopyId == id);
      return View(model);
    }
    public ActionResult Delete(int id)
    {
      var thisCopy = _db.Copies.FirstOrDefault(x => x.CopyId == id);
      return View(thisCopy);
    }
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisCopy = _db.Copies.FirstOrDefault(x => x.CopyId == id);
      _db.Copies.Remove(thisCopy);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Edit(int id)
    {
      var thisCopy = _db.Copies.FirstOrDefault(Copies => Copies.CopyId == id);
      return View(thisCopy);
    }
    [HttpPost]
    public ActionResult Edit(Copy Copy)
    {
      _db.Entry(Copy).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}