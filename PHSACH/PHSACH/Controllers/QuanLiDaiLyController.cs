using PHSACH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PHSACH.Controllers
{
    public class QuanLiDaiLyController : Controller
    {
        //
        // GET: /QuanLiDaiLy/
        PhatHanhSachEntities db = new PhatHanhSachEntities(); 
        public ActionResult Index()
        {
            var list = db.DAILY;
            var sp = db.DAILY.FirstOrDefault();
            return View(list);
        }
        [HttpGet]
        public ActionResult ThemDaiLy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemDaiLy(DAILY s)
        {
            string rexTen = @"^[a-zA-Z0-9_@.-]+$";
            string rexDiachi = @"^[A-Za-z0-9]+(?:\s[A-Za-z0-9'_-]+)+$";
            string rexSoDT = @"^[0][9]{9,12}$";
            string rexemail = @"^([\w\.\-\_]+)@([\w\-]+)((\.(\w){2,3})+)$";// trong demo k dùng

            Boolean ten = Regex.IsMatch(s.Ten, rexTen);
            Boolean diachi = Regex.IsMatch(s.DiaChi, rexDiachi);
            Boolean soDT = Regex.IsMatch(s.SoDT, rexSoDT);


            if (ten || diachi || soDT)
            {
                s.TrangThai = true;
                db.DAILY.Add(s);
                db.SaveChanges();
                return RedirectToAction("IndexDaiLy", "Admin");

            }
            else
            {
                if (!ten)
                    ViewBag.mes = "Tên không được chứa ký tự đặc biệt";
                else if (!diachi)
                    ViewBag.mes = "Địa chỉ không hợp lệ";
                else if (!soDT)
                    ViewBag.mes = "Số điện thoại không hợp lệ";

                return View();
            }

        }

        [HttpGet]
        public ActionResult CapNhatDaiLy(int id)
        {
            var daily = db.DAILY.Find(id);
            return View(daily);
        }

        [HttpPost]
        public ActionResult CapNhatDaiLy(DAILY dl, int id)
        {
            DAILY dl1 = new DAILY();
            using (var dbphs = new PhatHanhSachEntities())
            {
                dl1 = dbphs.DAILY.Where(d => d.MaDL == id).FirstOrDefault();

            }
            if (dl1 != null)
            {

                dl1.MaDL = id;
                dl1.Ten = dl.Ten;
                dl1.SoDT = dl.SoDT;
                dl1.DiaChi = dl.DiaChi;
                dl1.TrangThai = dl.TrangThai;
            }
            using (var dbps = new PhatHanhSachEntities())
            {

                dbps.Entry(dl1).State = System.Data.Entity.EntityState.Modified;
                //int i = db.Database.ExecuteSqlCommand("Update DAILY Set Ten="+dl.Ten+" ,DiaChi="+dl.DiaChi+" ,SoDT="+dl.SoDT+" ,TrangThai="+dl.TrangThai+ "Where MaDL=" + dl.MaDL);
                dbps.SaveChanges();
            }
            return RedirectToAction("Index", "QưanLiDaiLy");
        }
        [HttpGet]
        public ActionResult XoaDaiLy(int id)
        {
            DAILY dl1 = new DAILY();
            using (var dbphs = new PhatHanhSachEntities())
            {
                dl1 = dbphs.DAILY.Where(d => d.MaDL == id).FirstOrDefault();
            }
            if (dl1 != null)
            {

                if (dl1.TrangThai == false) dl1.TrangThai = true;
                else dl1.TrangThai = false;
            }
            using (var dbps = new PhatHanhSachEntities())
            {

                dbps.Entry(dl1).State = System.Data.Entity.EntityState.Modified;
                //int i = db.Database.ExecuteSqlCommand("Update DAILY Set Ten="+dl.Ten+" ,DiaChi="+dl.DiaChi+" ,SoDT="+dl.SoDT+" ,TrangThai="+dl.TrangThai+ "Where MaDL=" + dl.MaDL);
                dbps.SaveChanges();
            }
            return RedirectToAction("Index", "QuanLiDaiLy");
        }

    }
}
