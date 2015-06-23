using EastRiverCommune.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EastRiverCommune.Entities
{
	[System.ComponentModel.DataAnnotations.MetadataType(typeof(Metadata._OrderList))]
	public partial class OrderList
	{

	}
}

namespace EastRiverCommune.Entities.Metadata
{
	public class _OrderList
	{
		public static bool MinDate(object value)
		{
			return false;
		}

		//[DataType(DataType.Date)]
		[DisplayName("提货日期")]
		[Required]
		[MinLength(10)]
		[MaxLength(10)]
		public DateTime DeliveryDate { get; set; }

		[DisplayName("提货方式")]
		[Required]
		public string DeliveryManner { get; set; }

		[DisplayName("联系人")]
		[Required]
		public string Man { get; set; }

		[DisplayName("联系电话")]
		[Required]
		//[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[Required]
		public virtual ICollection<OrderItem> OrderItems { get; set; }
	}

}
