using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using MVC_OnlineStore.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace MVC_OnlineStore.Controllers
{
    public class CartController : Controller
    {
        StoreContext db = new StoreContext();
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

                model.Quantity = quantity;
                model.Price = price;

            }
            else
            {
                model.Quantity = 0;
                model.Price = 0;
            }

            return PartialView("_CartPartial", model);
        }

        public ActionResult AddToCartPartial(int id)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel> ?? new List<CartViewModel>();

            CartViewModel model = new CartViewModel();

            Product product = db.Products.Find(id);

            var productInCart = cart.FirstOrDefault(x=> x.ProductId == id);

            if(productInCart == null)
            {
                cart.Add(new CartViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Quantity = 1,
                    Price = product.Price,
                    Image = product.ImageName
                });
            }
            else
            {
                productInCart.Quantity++;
            }

            int quantity = 0;
            double price = 0;

            foreach (var item in cart)
            {
                quantity += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = quantity;
            model.Price = price;

            Session["cart"] = cart;

            return PartialView("_AddToCartPartial", model);
        }

        public JsonResult IncrementProduct(int productId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;

            CartViewModel model = cart.FirstOrDefault(x => x.ProductId == productId);

            model.Quantity++;

            var result = new { qty = model.Quantity, price = model.Price};

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DecrementProduct(int productId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;

            CartViewModel model = cart.FirstOrDefault(x => x.ProductId == productId);

            if(model.Quantity > 1)
            {
                model.Quantity--;
            }
            else
            {
                model.Quantity = 0;
                cart.Remove(model);
            }

            var result = new { qty = model.Quantity, price = model.Price };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void RemoveProduct(int productId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;

            CartViewModel model = cart.FirstOrDefault(x => x.ProductId == productId);

            cart.Remove(model);
        }

    }
}