using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EastRiverCommune.Entities
{
	[System.ComponentModel.DataAnnotations.MetadataType(typeof(Metadata._OrderItem))]
	public partial class OrderItem
	{

	}
}

namespace EastRiverCommune.Entities.Metadata
{
	public class _OrderItem
	{
		[Required]
		public string CommodityID { get; set; }

		[DisplayName("购买个数")]
		[RangeAttribute(1, int.MaxValue, ErrorMessage = "{0}最少{1}个最多{2}个")]
		public int Count { get; set; }
	}

}
