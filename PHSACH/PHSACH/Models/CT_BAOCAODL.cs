//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PHSACH.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CT_BAOCAODL
    {
        public int IdBCDL { get; set; }
        public Nullable<int> MaBCDL { get; set; }
        public Nullable<int> MaSach { get; set; }
        public Nullable<int> SoLuongBan { get; set; }
        public Nullable<int> DonGiaBan { get; set; }
        public Nullable<int> ThanhTien { get; set; }
    
        public virtual BAOCAODL BAOCAODL { get; set; }
        public virtual SACH SACH { get; set; }
    }
}