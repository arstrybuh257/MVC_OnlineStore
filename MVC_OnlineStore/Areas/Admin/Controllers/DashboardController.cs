using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using System.Web.Mvc;
using System.Linq;
using MVC_OnlineStore.Models.ViewModels;
using System.Collections.Generic;

namespace MVC_OnlineStore.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        StoreContext db = new StoreContext();

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            string userName = User.Identity.Name;

            User user = db.Users.FirstOrDefault(x => x.Username == userName);

            AdminViewModel model = new AdminViewModel(user);

            Session["Theme"] = model.Theme;

            return View(model);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            string userName = User.Identity.Name;
            User user = db.Users.FirstOrDefault(x=> x.Username == userName);

            AdminViewModel model = new AdminViewModel(user);

            Session["Theme"] = model.Theme;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(AdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = db.Users.Find(model.Id);

            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.EmailAdress = model.EmailAdress;
            user.Theme = model.Theme;

            if(db.Users.Count(x=> x.Username == model.Username) > 1)
            {
                ModelState.AddModelError("", "Данное имя пользователя уже занято.");
                return View(model);
            }

            if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "пароли не совпадают.");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.Password = model.Password;
            }
            user.Theme = model.Theme;
            Session["Theme"] = model.Theme;

            db.SaveChanges();

            TempData["Message"] = "Страница была успешно изменена.";

            return RedirectToAction("EditProfile");
        }


    }
}