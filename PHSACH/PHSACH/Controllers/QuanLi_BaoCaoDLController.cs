using PHSACH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PHSACH.Controllers
{
    public class QuanLi_BaoCaoDLController : Controller
    {
        private PhatHanhSachEntities db = new PhatHanhSachEntities();
        public static List<SACH> listSach;

        // GET: BAOCAODLs
        public ActionResult Index()
        {
            var bAOCAODLs = db.BAOCAODL;
            return View(bAOCAODLs.ToList());
        }

        // GET: BAOCAODLs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAOCAODL bAOCAODL = db.BAOCAODL.Find(id);
            if (bAOCAODL == null)
            {
                return HttpNotFound();
            }
            return View(bAOCAODL);
        }

        // GET: BAOCAODLs/Create
        public ActionResult Create()
        {
            ViewBag.MaDL = new SelectList(db.DAILY, "MaDL", "Ten");
            ViewBag.MaSach = new SelectList(db.SACH, "MaSach", "TenSach");
            return View();
        }

        // POST: BAOCAODLs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "BAOCAODL")] BAOCAODL BAOCAODL,
                                    [Bind(Prefix = "ct")] CT_BAOCAODL[] CT_BAOCAODL)
        {
            if (ModelState.IsValid)
            {
                int idbaocaodl = 1;
                if (db.BAOCAODL.Any())
                    idbaocaodl = db.BAOCAODL.Max(o => o.MaBCDL) + 1;
                int idctbcdl = 1;

                foreach (CT_BAOCAODL ct in CT_BAOCAODL)
                {
                    ct.MaBCDL = idbaocaodl;
                    idctbcdl++;
                    TONKHODL tkdl = db.TONKHODL.FirstOrDefault(o => o.MaDL == BAOCAODL.MaDL && o.MaSach == ct.MaSach);
                    if (tkdl == null || (tkdl != null && tkdl.SLChuaBan == 0))
                    {
                        ModelState.AddModelError("", "Sách trả không nằm trong mục sách xuất cho đại lý");
                        return View();
                    }
                    else
                    {
                        if (tkdl != null && tkdl.SLChuaBan >= ct.SoLuongBan)
                        {
                            tkdl.SLChuaBan = tkdl.SLChuaBan - ct.SoLuongBan;
                            db.Entry(tkdl).State = EntityState.Modified;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Số sách trả lớn hơn số sách xuất cho đại lý");
                            ViewBag.MaDL = new SelectList(db.DAILY, "MaDL", "Ten", BAOCAODL.MaDL);
                            ViewBag.MaSach = new SelectList(db.SACH, "MaSach", "TenSach");
                            return View();
                        }
                    }
                }
                db.BAOCAODL.Add(BAOCAODL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDL = new SelectList(db.DAILY, "MaDL", "Ten", BAOCAODL.MaDL);
            ViewBag.MaSach = new SelectList(db.SACH, "MaSach", "TenSach");
            return View();
        }

        // GET: BAOCAODLs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAOCAODL bAOCAODL = db.BAOCAODL.Find(id);
            if (bAOCAODL == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDL = new SelectList(db.DAILY, "MaDL", "Ten", bAOCAODL.MaDL);
            return View(bAOCAODL);
        }

        // POST: BAOCAODLs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBCDL,MaDL,ThoiGian,ThanhToan,TrangThai")] BAOCAODL bAOCAODL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bAOCAODL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDL = new SelectList(db.DAILY, "MaDL", "Ten", bAOCAODL.MaDL);
            return View(bAOCAODL);
        }

        // GET: BAOCAODLs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAOCAODL bAOCAODL = db.BAOCAODL.Find(id);
            if (bAOCAODL == null)
            {
                return HttpNotFound();
            }
            return View(bAOCAODL);
        }

        // POST: BAOCAODLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BAOCAODL bAOCAODL = db.BAOCAODL.Find(id);
            db.BAOCAODL.Remove(bAOCAODL);
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

        [HttpPost]
        public JsonResult Search(string prefix)
        {
            var sach = (from s in db.SACH
                        where s.TenSach.Contains(prefix)
                        select new
                        {
                            label = s.TenSach,
                            id = s.MaSach
                        }).ToList();

            return Json(sach);
        }
    }
}
