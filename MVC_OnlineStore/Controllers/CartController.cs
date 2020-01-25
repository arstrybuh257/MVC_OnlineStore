using MVC_OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_OnlineStore.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<CartViewModel> ?? new List<CartViewModel>();

            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Ваша корзина пуста.";
                return View();
            }

            double total = 0;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.Total = total;

            return View(cart);
        }

        public ActionResult CartPartial()
        {
            CartViewModel model = new CartViewModel();

            int quantity = 0;
            double price = 0;

            if (Session["cart"] != null)
            {
                var list = (List<CartViewModel>) Session["cart"];

                foreach (var item in list)
                {
                    quantity += item.Quantity;
                    price += item.Quantity * item.Price;
                }
            }
            else
            {
                model.Quantity = 0;
                model.Price = 0;
            }

            return PartialView("_CartPartial", model);
        }
    }
}