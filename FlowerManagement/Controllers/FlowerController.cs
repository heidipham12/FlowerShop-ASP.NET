using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlowerManagement.Controllers
{
    public class FlowerController : Controller
    {
        FlowerDBDataContext _context = new FlowerDBDataContext();
        // GET: Flower
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(file.FileName);
                string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
                file.SaveAs(destFile);
            }
            return View();
        }


        public ActionResult ListFlowers()
        {
            var flowers = _context.Flower_tables.ToList();
            return Json(flowers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] Flower_table flower)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(flower.ImagePath);

                flower.ImagePath = fileName;
                _context.Flower_tables.InsertOnSubmit(flower);
                _context.SubmitChanges();
            }
            return Json(flower, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var flower = _context.Flower_tables.ToList().Find(f => f.ID == id);
            if (flower != null)
            {
                _context.Flower_tables.DeleteOnSubmit(flower);
                _context.SubmitChanges();
            }
            return Json(flower, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int id)
        {
            var flower = _context.Flower_tables.ToList().Find(f => f.ID == id);

            string rootPath = Server.MapPath("~/");
            string fileName = System.IO.Path.GetFileName(flower.ImagePath);
            string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
            flower.ImagePath = destFile;

            return Json(flower, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Update(Flower_table flower)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(flower.ImagePath);
                flower.ImagePath = fileName;

                Flower_table f = (from p in _context.Flower_tables
                            where p.ID == flower.ID
                            select p).SingleOrDefault();

                f.FlowerName = flower.FlowerName;
                f.ImagePath = flower.ImagePath;
                f.Description = flower.Description;
                _context.SubmitChanges();

            }
            return Json(flower, JsonRequestBehavior.AllowGet);
        }
    }
}
    
            