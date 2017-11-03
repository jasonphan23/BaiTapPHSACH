using PHSACH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHSACH.Controllers
{
    public class QuanLi_CongNoNXBController : Controller
    {
        //
        // GET: /QuanLi_CongNoNXB/

        PhatHanhSachEntities db = new PhatHanhSachEntities();
        // GET: QuanLi_CongNo_DL
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CongNoNXB(DateTime date_congnonxb)
        {
            List<CONGNO_NXB> lst_congno_nxb = new List<CONGNO_NXB>();
            List<NHAXUATBAN> lst_nxb = new List<NHAXUATBAN>();
            lst_nxb = db.NHAXUATBAN.ToList();
            foreach (NHAXUATBAN nxb in lst_nxb)
            {
                CONGNO_NXB congno_nxb = new CONGNO_NXB();
                congno_nxb = db.CONGNO_NXB.Where(x => x.ThoiGian <= date_congnonxb.Date && x.MaNXB == nxb.MaNXB).OrderByDescending(X => X.ThoiGian).FirstOrDefault();
                if (congno_nxb == null)
                {
                    CONGNO_NXB cn_rong = new CONGNO_NXB();
                    cn_rong.NHAXUATBAN = nxb;
                    cn_rong.TienNo = 0;
                    lst_congno_nxb.Add(cn_rong);
                }
                else
                {
                    lst_congno_nxb.Add(congno_nxb);
                }
                
            }
            ViewBag.NgayCongNo = date_congnonxb.Date;
            return View(lst_congno_nxb);

        }
    }
}
