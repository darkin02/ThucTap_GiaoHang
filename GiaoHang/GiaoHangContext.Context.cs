﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GiaoHangEntities : DbContext
    {
        public GiaoHangEntities()
            : base("name=GiaoHangEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<HangHoa> HangHoas { get; set; }
        public virtual DbSet<LoaiHH> LoaiHHs { get; set; }
    
        public virtual ObjectResult<getNameHangHoa_Result> getNameHangHoa(string tenHH)
        {
            var tenHHParameter = tenHH != null ?
                new ObjectParameter("TenHH", tenHH) :
                new ObjectParameter("TenHH", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getNameHangHoa_Result>("getNameHangHoa", tenHHParameter);
        }
    }
}