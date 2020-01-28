using MVC_OnlineStore.Areas.Admin.Infrastructure;
using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using MVC_OnlineStore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
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
            pages = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageViewModel(x)).ToList();

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

            if (db.Pages.Any(x => x.Title == model.Title || x.Description == model.Description))
            {
                ModelState.AddModelError("", "Такая страница уже существует.");
                return View(model);
            }

            newPage.Description = description;
            newPage.Body = model.Body;
            if (model.Description == "home")
            {
                newPage.Sorting = 2;
            }
            else newPage.Sorting = 100;
            newPage.HasSlidebar = model.HasSlidebar;

            db.Pages.Add(newPage);
            db.SaveChanges();

            TempData["Message"] = "Новая страница была успешно добавлена.";
            return RedirectToAction("Index");
        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            Page page = db.Pages.Find(id);

            if (page == null)
            {
                return Content("Страница не найдена.");
            }

            PageViewModel model = new PageViewModel(page);

            return View(model);
        }

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string description;

            Page newPage = db.Pages.Find(model.PageId);
            if (newPage != null)
            {
                newPage.Title = model.Title.ToUpper();

                if (string.IsNullOrWhiteSpace(model.Description))
                {
                    description = model.Title.ToLower();
                }
                else description = model.Description;

                if (db.Pages.Where(x => x.PageId != model.PageId).Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "Такая страница уже существует.");
                    return View(model);
                }

                newPage.Description = description;
                newPage.Body = model.Body;
                if (model.Description == "home")
                {
                    newPage.Sorting = -2;
                }
                else newPage.Sorting = -1;

                newPage.HasSlidebar = model.HasSlidebar;
            }

            db.SaveChanges();

            TempData["Message"] = "Страница была успешно изменена.";
            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult PageDetails(int id)
        {
            Page page = db.Pages.Find(id);

            if (page == null)
            {
                return Content("Страница не найдена.");
            }

            PageViewModel model = new PageViewModel(page);

            return View(model);
        }

        // GET: Admin/Pages/DeletePage
        [HttpGet]
        public ActionResult DeletePage(int id)
        {
            Page page = db.Pages.Find(id);

            if (page == null)
            {
                return Content("Страница не найдена.");
            }

            db.Pages.Remove(page);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(ReorderHelper rh)
        {
            int[] ids = rh.id.ToArray();
            int count = 1;
            Page page;
            foreach (var pageId in ids)
            {
                page = db.Pages.Find(pageId);
                if (page != null) page.Sorting = count;

                db.SaveChanges();

                count++;
            }
        }

        // GET: Admin/Pages/EditSideBar
        [HttpGet]
        public ActionResult EditSideBar()
        {
            SideBarViewModel model;

            SideBar sb = db.SideBars.Find(1);

            model = new SideBarViewModel(sb);

            return View(model);
        }

        // POST: Admin/Pages/EditSideBar
        [HttpPost]
        public ActionResult EditSideBar(SideBarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SideBar newModel = db.SideBars.Find(model.Id);
            if (newModel != null) newModel.Body = model.Body;
            db.SaveChanges();

            TempData["Message"] = "Cайдбар был успешно изменен.";
            return RedirectToAction("EditSideBar");
        }
    }
}