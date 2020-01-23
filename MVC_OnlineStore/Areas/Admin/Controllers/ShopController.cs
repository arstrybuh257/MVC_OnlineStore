using MVC_OnlineStore.Areas.Admin.Infrastructure;
using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using MVC_OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using PagedList;

namespace MVC_OnlineStore.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        StoreContext db = new StoreContext();

        #region Actions for categories
        // GET: Admin/Shop
        public ActionResult Index()
        {
            List<CategoryViewModel> categories = db.Categories.ToArray().
                OrderBy(x => x.Sorting).Select(x => new CategoryViewModel(x)).ToList();


            return View(categories);
        }

        //Post: Admin/Shop/AddCategory
        [HttpPost]
        public string AddCategory(string catName)
        {
            string id;

            if (db.Categories.Any(x => x.Name == catName))
            {
                return "titletaken";
            }
            Category model = new Category();
            model.Name = catName;
            model.Description = catName.Replace(' ', '-').ToLower();
            model.Sorting = 100;
            db.Categories.Add(model);
            db.SaveChanges();

            id = model.Id.ToString();

            return id;
        }

        // GET: Admin/Shop/DeleteCategory
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);

            if (category == null)
            {
                return Content("Страница не найдена.");
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            TempData["Message"] = "Категория была успешно удалена.";
            return RedirectToAction("Index");
        }

        //Post: Admin/Shop/RenameCategories
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            Category category = db.Categories.Find(id);

            if (db.Categories.Any(x => x.Name == newCatName))
            {
                return "titletaken";
            }
            category.Name = newCatName;
            category.Description = newCatName.Replace(' ', '-').ToLower();
            db.SaveChanges();
            return "success";
        }

        //Post: Admin/Shop/ReorderCategories
        public void ReorderCategories(ReorderHelper rh)
        {
            int[] ids = rh.id.ToArray();
            int count = 1;
            Category category;
            foreach (var categoryId in ids)
            {
                category = db.Categories.Find(categoryId);
                category.Sorting = count;

                db.SaveChanges();

                count++;
            }
        }
        #endregion

        #region Actions for products
        // GET: Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            ProductViewModel model = new ProductViewModel();

            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            return View(model);
        }

        // POST: Admin/Shop/AddProduct
        [HttpPost]
        public ActionResult AddProduct(ProductViewModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                return View(model);
            }

            if (db.Products.Any(x => x.Name == model.Name))
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                ModelState.AddModelError("", "Продукт с таким названием уже существует.");
                return View(model);
            }

            int productId;

            Product newModel = new Product();
            newModel.Name = model.Name;
            newModel.Description = model.Description;
            newModel.ShortInfo = model.Name.Replace(" ", "-").ToLower();
            newModel.Price = model.Price;
            newModel.Category = db.Categories.Where(x => x.Id == model.CategoryId).Select(x => x).FirstOrDefault();

            db.Products.Add(newModel);
            db.SaveChanges();

            productId = newModel.ProductId;
            TempData["Message"] = "Продукт был успешно добавлен.";

            //Image uploading
            var originalDirectiory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));
            var pathString1 = Path.Combine(originalDirectiory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectiory.ToString(), "Products\\" + productId.ToString());
            var pathString3 = Path.Combine(originalDirectiory.ToString(), "Products\\" + productId.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectiory.ToString(), "Products\\" + productId.ToString() + "\\Thumbs");
            var pathString5 = Path.Combine(originalDirectiory.ToString(), "Products\\" + productId.ToString() + "\\Thumbs");

            if (!Directory.Exists(pathString1)) Directory.CreateDirectory(pathString1);
            if (!Directory.Exists(pathString2)) Directory.CreateDirectory(pathString2);
            if (!Directory.Exists(pathString3)) Directory.CreateDirectory(pathString3);
            if (!Directory.Exists(pathString4)) Directory.CreateDirectory(pathString4);
            if (!Directory.Exists(pathString5)) Directory.CreateDirectory(pathString5);

            if (file != null && file.ContentLength > 0)
            {
                string type = file.ContentType.ToLower();

                if (type != "image/jpg"
                    && type != "image/jpg"
                    && type != "image/jpeg"
                    && type != "image/png"
                    && type != "image/svg"
                    && type != "image/gif")
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "Изображение не было загружено. Неправильный формат файла.");
                    return View(model);
                }

                string imageName = file.FileName;
                Product product = db.Products.Find(productId);
                product.ImageName = imageName;
                db.SaveChanges();

                var path = string.Format($"{pathString2}\\{imageName}");
                var path2 = string.Format($"{pathString3}\\{imageName}");

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            return RedirectToAction("AddProduct");
        }

        // GET: Admin/Shop/Products
        [HttpGet]
        public ActionResult Products(int? page, int? catId)
        {
            List<ProductViewModel> products;

            int pageNum = page ?? 1;

            products = db.Products.ToArray().
                Where(x => x.Category.Id == catId || catId == 0 ||
                catId == null).Select(x=> new ProductViewModel(x)).ToList();

            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            ViewBag.SelectedCat = catId.ToString();


            //Используем плагин PagedListMVC
            var onePageOfProducts = products.ToPagedList(pageNum, 3);
            ViewBag.onePageOfProducts = onePageOfProducts;

            return View(products);
        }

        // GET: Admin/Shop/EditProduct
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            Product product = db.Products.Find(id);

            if (product == null)
            {
                return Content("Данный продукт не обнаружен!");
            }

            ProductViewModel model = new ProductViewModel(product);

            model.Categories = new SelectList(db.Categories, "Id", "Name");
            model.Images = Directory
                .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Thumbs"))
                .Select(x => Path.GetFileName(x));

            return View(model);

        }

        // POST: Admin/Shop/EditProduct
        [HttpPost]
        public ActionResult EditProduct(ProductViewModel model, HttpPostedFileBase file)
        {
            int id   = model.ProductId;
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            model.Images = Directory
                .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Thumbs"))
                .Select(x => Path.GetFileName(x));

            if (!ModelState.IsValid)
            {             
                return View(model);
            }

            if (db.Products.Where(x=> x.ProductId != id).Any(x=>x.Name == model.Name))
            {
                ModelState.AddModelError("", "Товар с таким именем уже существует!");
                return View(model);
            }

            Product product = db.Products.Find(id);
            product.Name = model.Name;
            product.ShortInfo = model.Name.Replace(" ", "-").ToLower();
            product.Description = model.Description;
            product.Price = model.Price;
            product.Category = db.Categories.Where(x => x.Id == model.CategoryId).Select(x => x).FirstOrDefault();
            product.ImageName = model.ImageName;

            db.SaveChanges();

            TempData["Message"] = "Продукт был успешно изменен";

            return RedirectToAction("EditProduct");
        }
        #endregion
    }

}

