using cStoreMVC.Doamin.Services;
using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uStoreMVC.Data.EF;
//using cStoreMVC.Doamin.Services;
using uStoreMVC.Domain;

namespace uStoreMVC.Controllers
{
    public class TripsController : Controller
    {
        private uStoreEntities1 db = new uStoreEntities1();

        // GET: Trips
        public ActionResult Index()
        {
            return View(db.Trips.ToList());
        }

        // GET: Trips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // GET: Trips/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TripId,TripName,TripDescription,Price,TipImage")] Trip trip, HttpPostedFileBase tripImageUpload) //Added HPFB parameter for file upload 
        {
            if (ModelState.IsValid)
            {
                #region File UpLoad region

                string imageName = "NoImage.jpg";
                if (tripImageUpload != null)
                {
                    //Find the extension
                    imageName = tripImageUpload.FileName;
                    string imgExtension = tripImageUpload.FileName.Substring(imageName.LastIndexOf("."));

                    string[] goodExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

                    if (goodExtensions.Contains(imgExtension.ToLower()))
                    {
                        imageName = Guid.NewGuid() + imgExtension; // Create unique file name with a guid. 

                        //Rather than save this file to the server directly, we'll use imageservice* to do it.
                        //make 2 copies, main & thumbnail.

                        //Lets make all the param that we need.
                        string imgPath = Server.MapPath("~/Content/Images/Trips/"); //Folder path.

                        Image convertedImage = Image.FromStream(tripImageUpload.InputStream); //Manipulates the file to be used in imageservices*

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
                trip.TripName = imageName;

                #endregion


                db.Trips.Add(trip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trip);
        }

        // GET: Trips/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TripId,TripName,TripDescription,Price,TipImage")] Trip trip, HttpPostedFileBase tripImageUpload)
        {
            if (ModelState.IsValid)
            {
                #region FILE UPLOAD (Edit) WITH IMAGE SERVICE
                //Took out the noimage default from the create. When the user edits it will not override the image that was uploaded by them.
                if (tripImageUpload != null)
                {
                    //Find the extension
                    //VARIATION FOR EDIT - VARIABLE DECLARATION HAPPENS HERE, ADD "STRING"DATATYPE
                    string imageName = tripImageUpload.FileName;
                    string imgExtension = tripImageUpload.FileName.Substring(imageName.LastIndexOf("."));

                    string[] goodExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

                    if (goodExtensions.Contains(imgExtension.ToLower()))
                    {
                        imageName = Guid.NewGuid() + imgExtension; // Create unique file name with a guid. 

                        //Rather than save this file to the server directly, we'll use imageservice* to do it.
                        //make 2 copies, main & thumbnail.

                        //Lets make all the param that we need.
                        string imgPath = Server.MapPath("~/Content/Images/Trips/"); //Folder path.

                        Image convertedImage = Image.FromStream(tripImageUpload.InputStream); //Manipulates the file to be used in imageservices*

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
                    trip.TipImage = imageName;
                }




                #endregion


                db.Entry(trip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trip);
        }

        // GET: Trips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trip trip = db.Trips.Find(id);
            db.Trips.Remove(trip);
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
