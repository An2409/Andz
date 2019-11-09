using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Services;

namespace Shop.WebUI.Controllers
{
    public class BasketsController : Controller
    {
        private IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            this._basketService = basketService;
        }

        // GET: Baskets
        public ActionResult Index()
        {
            var model = _basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            _basketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            _basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = _basketService.GetBasketSummary(this.HttpContext);
            return PartialView(basketSummary);
        }
    }
}