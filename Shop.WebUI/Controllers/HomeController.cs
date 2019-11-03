using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Contracts;
using Core.Models;
using Core.ViewModels;

namespace Shop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBaseRepository<Product> product;
        private IBaseRepository<ProductCategory> productCategories;

        public HomeController(IBaseRepository<Product> productContext, IBaseRepository<ProductCategory> productCategoryContext)
        {
            product = productContext;
            productCategories = productCategoryContext;
        }

        public ActionResult Index(string Category = null)
        {
            List<Product> products;
            List<ProductCategory> productCategory = productCategories.Collection().ToList();
            if (Category == null)
            {
                products = product.Collection().ToList();
            }
            else
            {
                products = product.Collection().Where(p => p.Category == Category).ToList();
            }
            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = productCategory;
            return View(model);
        }

        public ActionResult Details(string Id)
        {
            Product prd = product.Find(Id);
            if (prd == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(prd);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}