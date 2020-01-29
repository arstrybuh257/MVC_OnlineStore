using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using MVC_OnlineStore.Models.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MVC_OnlineStore.Controllers
{
    public class ShopController : Controller
    {
        StoreContext db = new StoreContext();
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }
        public ActionResult CategoryMenuPartial()
        {
            List<CategoryViewModel> categories = db.Categories.ToArray().OrderBy(x => x.Sorting)
                .Select(x => new CategoryViewModel(x)).ToList();

            return PartialView("_CategoryMenuPartial", categories);
        }

        //GET: Shop/Category/name
        public ActionResult Category(string name, string searchString)
        {
            List<ProductViewModel> products;

            if(name == null)
            {
                ViewBag.CategoryName = "Товары";
                products = string.IsNullOrEmpty(searchString) ? db.Products.ToArray().Select(x => new ProductViewModel(x)).ToList()
                    : db.Products.ToArray().Where(x => x.Name.ToLower().Contains(searchString.ToLower())).Select(x => new ProductViewModel(x)).ToList();
                return View(products);
            }
            Category category = db.Categories.Where(x => x.Description == name).FirstOrDefault();

            products = string.IsNullOrEmpty(searchString) ?
                db.Products.ToArray()
                .Where(x => x.Category.Id == category.Id)
                .Select(x => new ProductViewModel(x)).ToList()
                : db.Products.ToArray()
                .Where(x => x.Category.Id == category.Id && x.Name.ToLower().Contains(searchString.ToLower()))
                .Select(x => new ProductViewModel(x)).ToList();

            ViewBag.CategoryName = category.Name;

            return View(products);
        }

        //GET: Shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            Product product = db.Products.Where(x => x.ShortInfo == name).FirstOrDefault();

            if(product == null)
            {
                return RedirectToAction("Index", "Shop");
            }

            int id = product.ProductId;

            ProductViewModel model = new ProductViewModel(product);

            model.Images = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                 .Select(fn => Path.GetFileName(fn));

            return View("ProductDetails", model);
        }


    }
}