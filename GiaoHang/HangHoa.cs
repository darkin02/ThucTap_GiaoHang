//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GiaoHang
{
    using System;
    using System.Collections.Generic;
    
    public partial class HangHoa
    {
        public int MaHH { get; set; }
        public string TenHH { get; set; }
        public int KG { get; set; }
        public byte[] Img { get; set; }
        public int SL { get; set; }
        public int MaLHH { get; set; }
    
        public virtual LoaiHH LoaiHH { get; set; }
    }
}
