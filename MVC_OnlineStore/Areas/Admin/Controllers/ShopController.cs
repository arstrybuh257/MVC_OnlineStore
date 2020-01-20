﻿using MVC_OnlineStore.Areas.Admin.Infrastructure;
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

        //Post: Admin/Shop/AddCategory
        [HttpPost]
        public string AddCategory(string catName)
        {
            string id;

            if(db.Categories.Any(x=>x.Name == catName))
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
    }
}