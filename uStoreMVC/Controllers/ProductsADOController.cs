using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uStoreMVC.ADO;
using uStoreMVC.Domain;

namespace uStoreMVC.Controllers
{
    public class ProductsADOController : Controller
    {
        ProductsDAL products = new ProductsDAL();

        // GET: ProductsADO
        public ActionResult Index()
        {
            ViewBag.ProductNames = products.GetProductNames();
            return View();
        }

        public ActionResult GetProducts()
        {
            return View(products.GetProducts());
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                products.CreateProduct(product);
                return RedirectToAction("GetProducts");
            }

            return View(product);
        }


        //public ActionResult UpdateProduct(int id)
        //{
        //    return View(products.GetProduct(id));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                products.UpdateProduct(product);
                return RedirectToAction("GetProducts");
            }
            return View(product);
        }

        public ActionResult DeleteProduct(int id)
        {
            products.DeleteProduct(id);
            return RedirectToAction("GetProducts");
        }


    }
}