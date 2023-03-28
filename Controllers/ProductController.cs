using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

using CheckMVC;
namespace CheckMVC.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            using (ProductDBEntities1 db = new ProductDBEntities1())
            {
                List<tblProduct> ProductList = (from data in db.tblProducts select data).ToList();
                return View(ProductList);
            }
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View(new tblProduct());
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(tblProduct product, HttpPostedFileBase postedfile)
        {
            try
            {
                // TODO: Add insert logic here
                string extension = Path.GetExtension(postedfile.FileName);
                if (extension.Equals(".jpg") || extension.Equals(".png"))
                   {
                        string filename = "IMG-" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + extension;
                        string savepath = Server.MapPath("~/Content/Images/");
                        postedfile.SaveAs(savepath + filename);
                        product.prdt_img = filename;

                        using (ProductDBEntities1 db = new ProductDBEntities1())
                        {
                            db.tblProducts.Add(product);
                            db.SaveChanges();
                        }
                   }
                else 
                   {
                       return Content("<h2> You can only upload jpg or png file ! </h2>");
                   }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            using(ProductDBEntities1 db = new ProductDBEntities1())
            {
                tblProduct product = (from data in db.tblProducts where data.prdt_id == id select data).Single();
                return View(product);
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(tblProduct product, HttpPostedFileBase postedfile)
        {
            try
            {
                // TODO: Add update logic here
                string filename = "";
                if (postedfile != null)
                {
                    string extension = Path.GetExtension(postedfile.FileName);
                    if (extension.Equals(".jpg") || extension.Equals(".png"))
                    {
                        filename = "IMG-" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + extension;
                        string savepath = Server.MapPath("~/Content/Images/");
                        postedfile.SaveAs(savepath + filename);
                    }
                }
                using (ProductDBEntities1 db = new ProductDBEntities1())
                {
                    tblProduct tbl = (from data in db.tblProducts where data.prdt_id == product.prdt_id select data).Single();
                    tbl.prdt_name = product.prdt_name;
                    tbl.prdt_price = product.prdt_price;
                    tbl.prdt_qty = product.prdt_qty;
                    if(filename != null)
                    {
                        tbl.prdt_img = filename;
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (ProductDBEntities1 db = new ProductDBEntities1())
            {
                db.tblProducts.Remove(db.tblProducts.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
