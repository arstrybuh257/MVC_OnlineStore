using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_OnlineStore.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        StoreContext db = new StoreContext();
        // GET: Admin/Shop
        public ActionResult Index()
        {
            List<CategoryViewModel> categories = db.Categories.ToArray().
                OrderBy(x => x.Sorting).Select(x => new CategoryViewModel(x)).ToList();
                

            return View(categories);
        }
    }
}