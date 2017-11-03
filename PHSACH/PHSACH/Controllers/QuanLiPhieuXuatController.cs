using PHSACH.Models.ViewModels;
using PHSACH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHSACH.Controllers
{
    public class QuanLiPhieuXuatController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();

        public ActionResult Index()
        {
            var list = db.PHIEUXUAT;
            return View(list);
        }

        [HttpGet]
        public ActionResult XuatSach()
        {

            if (Session["DS_Sach"] == null)
                Session["DS_Sach"] = new List<CT_PhieuXuatViewModel>();

            ViewBag.DS_DaiLy = new SelectList(db.DAILY.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            //controller xử lí xong sẽ sinh gd
            return View();
        }

        [HttpPost]
        public ActionResult XuatSach(DAILY dl, FormCollection f)
        {
            //Lưu phiếu xuất
            PHIEUXUAT px = new PHIEUXUAT();
            px.MaDL = int.Parse(f["MaDL"].ToString());
            String[] temp = f["NgayXuat"].ToString().Split('-');
            DateTime date = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            px.NgayXuat = date;
            px.TrangThai = false;
            db.PHIEUXUAT.Add(px);
            db.SaveChanges();

            //Duyệt qua các phần tử của mảng để lưu CT_PhieuXuat
            int? TongTien = 0;
            foreach (CT_PhieuXuatViewModel ct in Session["DS_Sach"] as List<CT_PhieuXuatViewModel>)
            {
                CT_PHIEUXUAT ctpx = new CT_PHIEUXUAT();
                ctpx.MaPX = px.MaPX;
                ctpx.MaSach = ct.MaSach;
                ctpx.SLXuat = ct.SLXuat;
                ctpx.DonGia = ct.DonGia;
                ctpx.ThanhTien = ctpx.SLXuat * ctpx.DonGia;
                TongTien += ctpx.ThanhTien;
                db.CT_PHIEUXUAT.Add(ctpx);
            }
            px.TongTien = TongTien;
            db.SaveChanges();

            //Lưu công nợ Đại lý
            CONGNO_DL congno = new CONGNO_DL();
            congno.MaDL = dl.MaDL;
            congno.ThoiGian = new DateTime(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[0]));
            congno.TienDaTra = 0;
            congno.TienNo = px.TongTien;
            db.CONGNO_DL.Add(congno);
            db.SaveChanges();
            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Search(string prefix)
        {
            var sach = (from s in db.SACH
                        where s.TenSach.Contains(prefix) && s.TrangThai == true
                        select new
                        {
                            label = s.TenSach,
                            val = s.MaSach
                        }).ToList();

            return Json(sach);
        }

        [HttpPost]
        public ActionResult ThemChiTiet(SACH sach, FormCollection f)
        {
            SACH s = db.SACH.SingleOrDefault(n => n.MaSach == sach.MaSach);
            CT_PhieuXuatViewModel ctpx = new CT_PhieuXuatViewModel();
            ctpx.MaSach = s.MaSach;
            ctpx.TenSach = s.TenSach;
            ctpx.DonGia = int.Parse(f["DonGia"]);
            ctpx.SLXuat = int.Parse(f["SLXuat"]);
            ctpx.ThanhTien = ctpx.DonGia * ctpx.SLXuat;
            ((List<CT_PhieuXuatViewModel>)Session["DS_Sach"]).Add(ctpx);
            ViewBag.DS_DaiLy = new SelectList(db.DAILY.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            return View("XuatSach");
            // thêm CT phiếu nhập vô table
        }

        public ActionResult XoaChiTiet(int MaSach)
        {
            ((List<CT_PhieuXuatViewModel>)Session["DS_Sach"]).RemoveAll(p => p.MaSach == MaSach);
            ViewBag.DS_DaiLy = new SelectList(db.DAILY.Where(n => n.TrangThai == true).ToList(), "MaDL", "Ten");
            return View("XuatSach");
        }

        public ActionResult XemChiTiet(int? MaPX)
        {
            var px = db.PHIEUXUAT.SingleOrDefault(n => n.MaPX == MaPX);
            ViewBag.DS_CTPhieuXuat = px.CT_PHIEUXUAT;
            return View(px);
        }

    }
}
