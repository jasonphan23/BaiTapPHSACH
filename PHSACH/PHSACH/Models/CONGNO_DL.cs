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
    
    public partial class CONGNO_DL
    {
        public int MaDL { get; set; }
        public System.DateTime ThoiGian { get; set; }
        public Nullable<int> TienNo { get; set; }
        public Nullable<int> TienDaTra { get; set; }
    
        public virtual DAILY DAILY { get; set; }
    }
}
