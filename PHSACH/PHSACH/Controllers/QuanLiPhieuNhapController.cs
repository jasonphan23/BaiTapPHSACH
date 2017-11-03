using PHSACH.Models.ViewModels;
using PHSACH.Models;
using PHSACH.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHSACH.Controllers
{
    public class QuanLiPhieuNhapController : Controller
    {
        PhatHanhSachEntities entities = new PhatHanhSachEntities();

        public ActionResult Index()
        {
            var list = entities.PHIEUNHAP.ToList();
            return View(list);
        }

        public ActionResult CT_PhieuNhap(int? MaPN)
        {
            var list = entities.CT_PHIEUNHAP.Where(n => n.MaPN == MaPN);
            return View(list);
        }

        public ActionResult NhapSach()
        {
            NHAXUATBAN nxb = new NHAXUATBAN();

            var getnxblist = entities.NHAXUATBAN.ToList();
            SelectList list = new SelectList(getnxblist, "MaNXB", "Ten");
            ViewBag.nxblistname = list;

            if (Session["listSach"] == null)
                Session["listSach"] = new List<SachViewModel>();

            return View();
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var sachlist = (from s in entities.SACH
                            where s.TenSach.Contains(prefix)
                            select new
                            {
                                label = s.TenSach,
                                val = s.MaSach
                            }).ToList();

            return Json(sachlist);
        }



        [HttpPost]
        public ActionResult ThemVaoBang(SachViewModel sachVM)
        {
            try
            {
                SACH sach = entities.SACH.Where(s => s.TenSach == sachVM.TenSach).FirstOrDefault();
                sachVM.MaSach = sach.MaSach;
                sachVM.TenSach = sach.TenSach;
                sachVM.GiaNhap = (int)sach.DonGiaNhap;
                ((List<SachViewModel>)Session["listSach"]).Add(sachVM);
                return RedirectToAction("NhapSach");
            }
            catch (NullReferenceException e)
            {
                throw new Exception("null value not allowed");
            }
            // Xu ly code truy xuat sach

        }

        public ActionResult XoaKhoiBang(int MaSach)
        {
            // Xu ly code truy xuat sach
            ((List<SachViewModel>)Session["listSach"]).RemoveAll(p => p.MaSach == MaSach);
            return RedirectToAction("NhapSach");
        }

        [HttpPost]
        public ActionResult LuuCSDL(SachViewModel sachVM)
        {
            PHIEUNHAP pn = new PHIEUNHAP();
            pn.NgayNhap = sachVM.NgayNhap;
            pn.MaNXB = sachVM.MaNXB;
            pn.TrangThai = true;

            var addedPN = entities.PHIEUNHAP.Add(pn);
            entities.SaveChanges();

            int tongTien = 0;

            foreach (var ct in (List<SachViewModel>)Session["listSach"])
            {
                int thanhTien = ct.GiaNhap * ct.SLNhap;
                tongTien += thanhTien;
                // Add ct phieu nhap
                CT_PHIEUNHAP ctpn = new CT_PHIEUNHAP();
                ctpn.MaPN = pn.MaPN;
                ctpn.MaSach = ct.MaSach;
                ctpn.SLNhap = ct.SLNhap;
                ctpn.DonGia = ct.GiaNhap;
                ctpn.ThanhTien = thanhTien;

                entities.CT_PHIEUNHAP.Add(ctpn);
            }

            // Update tong tien 
            addedPN.TongTien = tongTien;

            // Update cong no NXB
            CONGNO_NXB cnNXB = new CONGNO_NXB();
            cnNXB.MaNXB = sachVM.MaNXB;
            cnNXB.ThoiGian = sachVM.NgayNhap;
            cnNXB.TienNo = tongTien;
            cnNXB.TienDaTra = 0;
            entities.CONGNO_NXB.Add(cnNXB);
            entities.SaveChanges();

            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemPhieuNhap(PHIEUNHAP pn, CT_PHIEUNHAP ctpn)
        {
            //Session["dsSach"] = new List<>();
            entities.CT_PHIEUNHAP.Add(ctpn);
            entities.SaveChanges();
            return RedirectToAction("Index", "NhapSach");
        }
    }
}
