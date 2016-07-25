using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class DetailsController : Controller
    {
        private AzureDBEntities db = new AzureDBEntities();
         
        public ActionResult Statistics()
        {
            return View();
        }
        public ActionResult ProjectML()
        {
            return View();
        }
        public ActionResult TitanicApp()
        {
            return View();
        }
        public ActionResult GenderSurvivalStats()
        {
            var data = (from Titanics in db.Titanics
                        where
                          Titanics.Gender != "" &&
                          Titanics.Survived == 1
                        group Titanics by new
                        {
                            Titanics.Gender
                        } into g
                        select new
                        {
                            Suvival_Count = g.Count(p => p.Survived != null)
                        }).Distinct();

            var chartData = new object[data.Count() + 1];
            chartData[0] = new object[]{
                    "Suvival_Count", 
                };

            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] { i.Suvival_Count };
            }

            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult SurvivalStats()
        {
          
            var data = from Titanics in db.Titanics
                       group Titanics by new
                       {
                           Titanics.Survived
                       } into g
                       select new
                       {
                           Suvival_Count = g.Count(p => p.Survived != null)
                       };

            var chartData = new object[data.Count() + 1];
            chartData[0] = new object[]{
                    "Suvival_Rate", 
                };

            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] { i.Suvival_Count };
            }

            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult AgePerEmbarkment()
        {
            var data = from Titanics in db.Titanics
                       where
                         Titanics.Passenger_class != null
                       group Titanics by new
                       {
                           Titanics.Passenger_class
                       } into g
                       select new
                       {
                           Passenger_Class = g.Key.Passenger_class,
                           Average_Age = (double?)g.Average(p => p.Age),
                           Minimum_Age = (int?)g.Min(p => p.Age),
                           Max_Age = (int?)g.Max(p => p.Age)
                       };
            
            var chartData = new object[data.Count() + 1];
            
            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] { i.Average_Age, i.Minimum_Age,i.Max_Age, i.Passenger_Class };
            }

            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult Index1()
        {

            return Json(db.Titanics.ToList(), JsonRequestBehavior.AllowGet);
        }
        // GET: Details


        // GET: Details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titanic titanic = db.Titanics.Find(id);
            if (titanic == null)
            {
                return HttpNotFound();
            }
            return View(titanic);
        }

        // GET: Details/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Survived,Passenger_class,Gender,Age,Embarkment")] Titanic titanic)
        {
            if (ModelState.IsValid)
            {
                db.Titanics.Add(titanic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(titanic);
        }

        // GET: Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titanic titanic = db.Titanics.Find(id);
            if (titanic == null)
            {
                return HttpNotFound();
            }
            return View(titanic);
        }

        // POST: Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Survived,Passenger_class,Gender,Age,Embarkment")] Titanic titanic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(titanic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(titanic);
        }

        // GET: Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titanic titanic = db.Titanics.Find(id);
            if (titanic == null)
            {
                return HttpNotFound();
            }
            return View(titanic);
        }

        // POST: Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Titanic titanic = db.Titanics.Find(id);
            db.Titanics.Remove(titanic);
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
