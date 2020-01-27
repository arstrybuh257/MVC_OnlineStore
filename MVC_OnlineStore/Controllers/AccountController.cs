using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using MVC_OnlineStore.Models.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_OnlineStore.Controllers
{
    public class AccountController : Controller
    {
        StoreContext db = new StoreContext();

        public object FormsAutentication { get; private set; }

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: Account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }

            if (db.Users.Any(x => x.Username.Equals(model.Username)))
            {
                ModelState.AddModelError("", $"Имя пользователя {model.Username} уже занято!");
                model.Username = "";
                return View("CreateAccount", model);
            }

            User user = new User()
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                EmailAdress = model.EmailAdress,
                Username = model.Username,
                Password = model.Password
            };

            db.Users.Add(user);
            db.SaveChanges();

            UserRole userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = 2
            };

            db.UserRoles.Add(userRole);
            db.SaveChanges();

            TempData["Message"] = "Вы были успешно зарегестрированы";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            string userName = User.Identity.Name;

            if (!string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("user-profile");
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isValid = db.Users.Any(x => x.Username.Equals(model.Login) && x.Password.Equals(model.Password));

            if (!isValid)
            {
                ModelState.AddModelError("", "Неверные учетные данные");
                return View(model);
            }

            User user = db.Users.FirstOrDefault(x => x.Username == model.Login);
            if (!string.IsNullOrEmpty(user.Theme))
            {
                Session["Theme"] = user.Theme;
            }
            else Session["Theme"] = "dark";

            FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
            return Redirect(FormsAuthentication.GetRedirectUrl(model.Login, model.RememberMe));
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult UserNavPartial()
        {
            string userName = User.Identity.Name;

            User user = db.Users.FirstOrDefault(x => x.Username == userName);

            UserNavPartialViewModel model = new UserNavPartialViewModel()
            {
                FirstName = user.FirstName,
                SecondName = user.SecondName
            };

            return PartialView("_UserNavPartial", model);
        }

        [ActionName("user-profile")]
        [HttpGet]
        public ActionResult UserProfile()
        {
            string userName = User.Identity.Name;

            UserProfileViewModel model;

            User user = db.Users.FirstOrDefault(x => x.Username == userName);

            model = new UserProfileViewModel(user);

            return View("UserProfile", model);
        }

        [ActionName("user-profile")]
        [HttpPost]
        public ActionResult UserProfile(UserProfileViewModel model)
        {
            bool userNameIsChanged = false;

            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Пароли не совпадают");
                }
            }

            /////////////////////////////////////

            string userName = User.Identity.Name;

            if (model.Username != userName)
            {
                userName = model.Username;
                userNameIsChanged = true;
            }

            if (db.Users.Where(x => x.Id != model.Id).Any(x => x.Username == model.Username))
            {
                ModelState.AddModelError("", $"Имя пользователя {model.Username} уже занято!");
                model.Username = "";
                return View("CreateAccount", model);
            }

            User user = db.Users.Find(model.Id);

            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.EmailAdress = model.EmailAdress;
            user.Username = model.Username;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = model.Password;
            }

            db.SaveChanges();

            TempData["Message"] = "Профиль был успешно изменен";

            if (!userNameIsChanged)
            {
                return View("UserProfile", model);
            }
            else
            {
                return RedirectToAction("Logout");
            }

        }


    }
}