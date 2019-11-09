using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Contracts;
using Core.ViewModels;

namespace Shop.Services
{
    public class BasketService : IBasketService
    {
        private IBaseRepository<Product> _productContext;
        private IBaseRepository<Basket> _basketContext;
        public const string BasketSessionName = "eCommerce";

        public BasketService(IBaseRepository<Product> productContext, IBaseRepository<Basket> basketContext)
        {
            this._basketContext = basketContext;
            this._productContext = productContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
        
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);
            Basket basket = new Basket();
            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = _basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            _basketContext.Insert(basket);
            _basketContext.Commit();
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);
            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quanity = 1
                };
                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quanity += 1;
            }

            _basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                basket.BasketItems.Remove(item);
                _basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            if (basket != null)
            {
                var result = (from b in basket.BasketItems
                    join p in _productContext.Collection() on b.ProductId equals p.Id
                    select new BasketItemViewModel()
                    {
                        Id = b.Id,
                        Quanity = b.Quanity,
                        ProductName = p.Name,
                        Image = p.Image,
                        Price = p.Price
                    }).ToList();
                return result;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0,0);
            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems
                    select item.Quanity).Sum();
                decimal? basketTotal = (from item in basket.BasketItems
                    join p in _productContext.Collection() on item.ProductId equals p.Id
                    select item.Quanity * p.Price).Sum();
                model.BasketCount = basketCount ?? 0;
                model.BasketTotal = basketTotal ?? decimal.Zero;
                return model;
            }
            else
            {
                return model;
            }
        }
    }
}
