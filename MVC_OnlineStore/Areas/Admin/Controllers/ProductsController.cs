using MVC_OnlineStore.Areas.Admin.Infrastructure;
using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using MVC_OnlineStore.Models.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using PagedList;

namespace MVC_OnlineStore.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        StoreContext db = new StoreContext();

        // GET: Admin/Shop/Index
        [HttpGet]
        public ActionResult Index(int? page, int? catId)
        {
            List<ProductViewModel> products;

            int pageNum = page ?? 1;

            products = db.Products.ToArray().
                Where(x => x.Category.Id == catId || catId == 0 ||
                           catId == null).Select(x => new ProductViewModel(x)).ToList();

            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            ViewBag.SelectedCat = catId.ToString();

            //Используем плагин PagedListMVC
            var onePageOfProducts = products.ToPagedList(pageNum, 3);
            ViewBag.onePageOfProducts = onePageOfProducts;

            return View(products);
        }

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
            ImageHelper helper = new ImageHelper(originalDirectiory, productId);

            helper.CreateDirectoriesIfNotExist(originalDirectiory, productId);
            
            if (file != null && file.ContentLength > 0)
            {
                string type = file.ContentType.ToLower();

                if (!ImageHelper.IsImage(type))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "Изображение не было загружено. Неправильный формат файла.");
                    return View(model);
                }

                string imageName = file.FileName;
                Product product = db.Products.Find(productId);
                if (product != null) product.ImageName = imageName;
                db.SaveChanges();

                helper.SaveFiles(imageName, file);
            }

            return RedirectToAction("AddProduct");
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
                .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                .Select(x => Path.GetFileName(x));

            return View(model);

        }

        // POST: Admin/Shop/EditProduct
        [HttpPost]
        public ActionResult EditProduct(ProductViewModel model, HttpPostedFileBase file)
        {
            int id = model.ProductId;
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            model.Images = Directory
                .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                .Select(x => Path.GetFileName(x));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (db.Products.Where(x => x.ProductId != id).Any(x => x.Name == model.Name))
            {
                ModelState.AddModelError("", "Товар с таким именем уже существует!");
                return View(model);
            }

            Product product = db.Products.Find(id);
            if (product != null)
            {
                product.Name = model.Name;
                product.ShortInfo = model.Name.Replace(" ", "-").ToLower();
                product.Description = model.Description;
                product.Price = model.Price;
                product.Category = db.Categories.Where(x => x.Id == model.CategoryId).Select(x => x).FirstOrDefault();
                product.ImageName = model.ImageName;

                db.SaveChanges();

                TempData["Message"] = "Продукт был успешно изменен";

                var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));
                ImageHelper helper = new ImageHelper(originalDirectory, id);
                if (file != null && file.ContentLength > 0)
                {
                    string type = file.ContentType.ToLower();

                    if (!ImageHelper.IsImage(type))
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Изображение не было загружено. Неправильный формат файла.");
                        return View(model);
                    }

                    //Image uploading
                    
                   helper.ChangeImages();

                    string imageName = file.FileName;

                    product = db.Products.Find(id);
                    if (product != null) product.ImageName = imageName;
                    db.SaveChanges();

                    helper.SaveFiles(imageName, file);
                }
            }

            return RedirectToAction("EditProduct");
        }

        // GET: Admin/Shop/DeleteProduct
        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);

            if (product == null)
            {
                return Content("Продукт не обнаружен");
            }

            db.Products.Remove(product);
            db.SaveChanges();

            var originalDirectiory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));
            var pathString1 = Path.Combine(originalDirectiory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathString1))
            {
                Directory.Delete(pathString1, true);
            }

            return RedirectToAction("Index");
        }

        public void SaveGalleryImages(int id)
        {
            foreach (var fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName.ToString()];

                var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));
                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

                if (file != null)
                {
                    var path1 = string.Format($"{pathString1}\\{file.FileName}"); 
                    var path2 = string.Format($"{pathString2}\\{file.FileName}");

                    file.SaveAs(path1);

                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200).Crop(1,1);
                    img.Save(path2);
                }
            }
        }

        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1)) 
                System.IO.File.Delete(fullPath1);
            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }
    }

}

