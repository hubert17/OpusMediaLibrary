using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OpusMediaLibrary.Models;

namespace OpusMediaLibrary.Controllers
{
    public class CloudTokenController : Controller
    {
        private OpusMediaContext db = new OpusMediaContext();

        // GET: CloudToken
        public ActionResult Index()
        {
            return View(db.CloudTokens.ToList());
        }

        // GET: CloudToken/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CloudToken cloudToken = db.CloudTokens.Find(id);
            if (cloudToken == null)
            {
                return HttpNotFound();
            }
            return View(cloudToken);
        }

        // GET: CloudToken/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CloudToken/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountName,Token,Inactive")] CloudToken cloudToken)
        {
            if (ModelState.IsValid)
            {
                db.CloudTokens.Add(cloudToken);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cloudToken);
        }

        // GET: CloudToken/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CloudToken cloudToken = db.CloudTokens.Find(id);
            if (cloudToken == null)
            {
                return HttpNotFound();
            }
            return View(cloudToken);
        }

        // POST: CloudToken/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountName,Token,Inactive")] CloudToken cloudToken)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cloudToken).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cloudToken);
        }

        // GET: CloudToken/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CloudToken cloudToken = db.CloudTokens.Find(id);
            if (cloudToken == null)
            {
                return HttpNotFound();
            }
            return View(cloudToken);
        }

        // POST: CloudToken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CloudToken cloudToken = db.CloudTokens.Find(id);
            db.CloudTokens.Remove(cloudToken);
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
