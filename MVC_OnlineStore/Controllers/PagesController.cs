using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using MVC_OnlineStore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVC_OnlineStore.Controllers
{
    public class PagesController : Controller
    {
        StoreContext db = new StoreContext();
        // GET: Index/{page}
        [HttpGet]
        public ActionResult Index(string page = "")
        {
            if(page == "" || page == "home")
            {
                page = "home";
            }

            Page model;
            PageViewModel pageViewModel;

            if (!db.Pages.Any(x => x.Description.Equals(page))){
                return RedirectToAction("Index", new {page = ""});
            }

            model = db.Pages.Where(x => x.Description.Equals(page)).FirstOrDefault();

            ViewBag.PageTitle = model.Title;

            ViewBag.SideBar = model.HasSlidebar ? "Yes" : "No";

            pageViewModel = new PageViewModel(model);
          
            return View(pageViewModel);
        }

        public ActionResult PagesMenuPartial()
        {
            List<PageViewModel> pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Description != "home")
                .Select(x => new PageViewModel(x)).ToList();

            return PartialView("_PagesMenuPartial", pageList);           
        }

        public ActionResult SideBarPartial()
        {
            SideBar model = db.SideBars.Find(1);

            SideBarViewModel sideBarViewModel = new SideBarViewModel(model);

            return PartialView("_SideBarPartial", sideBarViewModel);
        }

    }
}