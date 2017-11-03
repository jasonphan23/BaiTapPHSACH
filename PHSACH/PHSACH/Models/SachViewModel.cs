using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHSACH.Models.ViewModels
{
    public class SachViewModel
    {
        public int MaNXB { get; set; }
        public DateTime NgayNhap { get; set; }
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public int GiaNhap { get; set; }
        public int SLNhap { get; set; }
    }
}