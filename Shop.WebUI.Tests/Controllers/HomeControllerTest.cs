using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Core.Contracts;
using Core.Models;
using Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.WebUI;
using Shop.WebUI.Controllers;
using Shop.WebUI.Tests.Mocks;

namespace Shop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            IBaseRepository<Product> productContext = new MockContext<Product>();
            IBaseRepository<ProductCategory> productCategoryContext = new MockContext<ProductCategory>();
            //productContext.Insert(new Product());
            HomeController controller = new HomeController(productContext, productCategoryContext);
            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel)result.ViewData.Model; 
            Assert.AreEqual(0, viewModel.Products.Count());
        }
    }
}
