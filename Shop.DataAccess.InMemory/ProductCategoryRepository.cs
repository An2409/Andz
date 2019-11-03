using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Shop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        private ObjectCache objCache = MemoryCache.Default;
        private List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = objCache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            objCache["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        public void Update(ProductCategory pc)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.Id == pc.Id);
            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = pc;
            }
            else
            {
                throw new Exception("Product category not found");
            }
        }

        public ProductCategory Find(string Id)
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == Id);
            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product category not found");
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == Id);
            if (productCategory != null)
            {
                productCategories.Remove(productCategory);
            }
            else
            {
                throw new Exception("Product deleted");
            }
        }
    }
}
