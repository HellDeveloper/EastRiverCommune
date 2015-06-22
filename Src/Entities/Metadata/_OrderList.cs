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
		//[DataType(DataType.Date)]
		[Required]
		[MinLength(10)]
		[MaxLength(10)]
		public DateTime DeliveryDate { get; set; }

		[Required]
		public string DeliveryManner { get; set; }

		[Required]
		public string Man { get; set; }

		[Required]
		//[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[Required]
		public virtual ICollection<OrderItem> OrderItems { get; set; }
	}

}
