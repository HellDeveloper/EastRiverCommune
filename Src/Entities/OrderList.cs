//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class OrderList
    {
        public OrderList()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public string ID { get; set; }
        public bool Enable { get; set; }
        public System.DateTime WhenModify { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public string DeliveryManner { get; set; }
        public string Man { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
