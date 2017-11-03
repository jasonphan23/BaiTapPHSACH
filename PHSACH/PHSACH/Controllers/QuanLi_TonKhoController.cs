using PHSACH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHSACH.Controllers
{
    public class QuanLi_TonKhoController : Controller
    {
        PhatHanhSachEntities db = new PhatHanhSachEntities();
        // GET: QuanLi_CongNo_DL
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TonKhoSach(DateTime date_tonkhosach)
        {
            List<TONKHO> lst_tk_s = new List<TONKHO>();
            List<SACH> lst_sach = new List<SACH>();
            lst_sach = db.SACH.ToList();
            foreach (SACH s in lst_sach)
            {
                TONKHO tk = new TONKHO();
                tk = db.TONKHO.Where(x => x.ThoiGian <= date_tonkhosach.Date && x.MaSach == s.MaSach).OrderByDescending(X => X.ThoiGian).FirstOrDefault();
                if (tk == null)
                {
                    TONKHO tk_rong = new TONKHO();
                    tk_rong.SACH = s;
                    tk_rong.SLTon = 0;
                    lst_tk_s.Add(tk_rong);
                }
                else
                {
                    lst_tk_s.Add(tk);
                }
            }
            ViewBag.NgayCongNo = date_tonkhosach.Date;
            return View(lst_tk_s);

        }

    }
}
