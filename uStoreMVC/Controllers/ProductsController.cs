using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uStoreMVC.Data.EF;

using cStoreMVC.Doamin.Services;//Added
using System.Drawing;//Added

namespace uStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private uStoreEntities1 db = new uStoreEntities1();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductDescription,Price,UnitsInStock,ProductImage")] Product product, HttpPostedFileBase proImageUpload) //Added HPFB ob
        {
            if (ModelState.IsValid)
            {
                #region File Upload (Create) w/ Image Service

                string imageName = "NoImage.jpg";
                if (proImageUpload != null)
                {
                    //Find the extension
                    imageName = proImageUpload.FileName;
                    string imgExtension = proImageUpload.FileName.Substring(imageName.LastIndexOf("."));

                    string[] goodExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

                    if (goodExtensions.Contains(imgExtension.ToLower()))
                    {
                        imageName = Guid.NewGuid() + imgExtension; // Create unique file name with a guid. 

                        //Rather than save this file to the server directly, we'll use imageservice* to do it.
                        //make 2 copies, main & thumbnail.

                        //Lets make all the param that we need.
                        string imgPath = Server.MapPath("~/Content/Images/Products/"); //Folder path.

                        Image convertedImage = Image.FromStream(proImageUpload.InputStream); //Manipulates the file to be used in imageservices*

                        //Choose max img size
                        int maxImageSize = 500;

                        //Set max size for thumbnails in pixels
                        int maxThumbSize = 100;

                        //Call ResizeImage() from ImageServices*
                        ImageServices.ResizeImage(imgPath, imageName, convertedImage, maxImageSize, maxThumbSize);

                    }
                    else
                    {
                        //handle invalid file type somehow...
                        imageName = "NoImage.jpg";
                    }
                }

                //Regardless of whether a file was uploaded, set the image name on the db record (hijack record)
                product.ProductImage = imageName;

                #endregion
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductDescription,Price,UnitsInStock")] Product product, HttpPostedFileBase proImageUpload)
        {
            if (ModelState.IsValid)
            {
                #region FILE UPLOAD (Edit) WITH IMAGE SERVICE
                //Took out the noimage default from the create. When the user edits it will not override the image that was uploaded by them.
                if (proImageUpload != null)
                {
                    //Find the extension
                    //VARIATION FOR EDIT - VARIABLE DECLARATION HAPPENS HERE, ADD "STRING"DATATYPE
                    string imageName = proImageUpload.FileName;
                    string imgExtension = proImageUpload.FileName.Substring(imageName.LastIndexOf("."));

                    string[] goodExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

                    if (goodExtensions.Contains(imgExtension.ToLower()))
                    {
                        imageName = Guid.NewGuid() + imgExtension; // Create unique file name with a guid. 

                        //Rather than save this file to the server directly, we'll use imageservice* to do it.
                        //make 2 copies, main & thumbnail.

                        //Lets make all the param that we need.
                        string imgPath = Server.MapPath("~/Content/Images/Products/"); //Folder path.

                        Image convertedImage = Image.FromStream(proImageUpload.InputStream); //Manipulates the file to be used in imageservices*

                        //Choose max img size
                        int maxImageSize = 500;

                        //Set max size for thumbnails in pixels
                        int maxThumbSize = 100;

                        //Call ResizeImage() from ImageServices*
                        ImageServices.ResizeImage(imgPath, imageName, convertedImage, maxImageSize, maxThumbSize);

                    }
                    else
                    {
                        //handle invalid file type somehow...
                        //VARIATION FOR EDIT -- JUST LEACE IT AT ORIGINAL FILE (HANDLED BY HIDDEN FIELD)
                        //imageName = "NoImage.jpg";
                    }
                    //VARIATION FOR EDIT-- MOVED RECORD HIJACK FOR BOOKIMAGE FIELD ASSIGNMENT TO HAPPEN ONLY IF USER UPLOADED A FILE.
                    product.ProductImage = imageName;
                }




                #endregion


                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
