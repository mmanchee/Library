// using System;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using System.Linq;
// using Microsoft.AspNetCore.Mvc;
// using Library.Models;

// namespace Library.Controllers
// {
//   public class CopiesController : Controller
//   {
//     private readonly LibraryContext _db;
//     public CopiesController(LibraryContext db)
//     {
//       _db=db;
//     }
//     public ActionResult Index()
//     {
//       List<Copy> model = _db.Copies.OrderBy(x=>x.CopyId).ToList();
//       return View(model);
//     }
//   }
// }