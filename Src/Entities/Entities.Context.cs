﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EastRiverCommune.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EastRiverEntities : DbContext
    {
        public EastRiverEntities()
            : base("name=EastRiverEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Directory> Directories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<TempData> TempDatas { get; set; }
        public DbSet<OrderList> OrderLists { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
