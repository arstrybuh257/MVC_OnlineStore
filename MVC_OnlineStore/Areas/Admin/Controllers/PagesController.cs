using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using MVC_OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_OnlineStore.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        StoreContext db = new StoreContext();
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageViewModel> pages;
            pages = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x=> new PageViewModel(x)).ToList();

            return View(pages);
        }
        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string description;

            Page newPage = new Page();
            newPage.Title = model.Title.ToUpper();

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                description = model.Title.ToLower();
            }
            else description = model.Description;

            if(db.Pages.Any(x=>x.Title == model.Title))
            {
                ModelState.AddModelError("", "Такая страница уже существует.");
                return View(model);
            }

            newPage.Description = description;
            newPage.Body = model.Body;
            newPage.Sorting = 100;

            db.Pages.Add(newPage);
            db.SaveChanges();

            TempData["Message"] = "Новая страница была успешно добавлена.";
            return RedirectToAction("Index");

            return View();
        }
    }
}