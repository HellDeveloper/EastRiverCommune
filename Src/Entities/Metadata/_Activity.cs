using EastRiverCommune.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EastRiverCommune.Entities
{
	[System.ComponentModel.DataAnnotations.MetadataType(typeof(Metadata._Activity))]
	public partial class Activity
	{

	}
}

namespace EastRiverCommune.Entities.Metadata
{
	public class _Activity
	{
		[DisplayName("活动标题")]
		[Required]
		public string Title { get; set; }

		public string CategoryID { get; set; }

		[DisplayName("活动内容")]
		[Required]
		public string Content { get; set; }

		[DisplayName("活动地址")]
		[Required]
		public string Address { get; set; }

		[DisplayName("报名开始时间")]
		public System.DateTime RegistBeginDate { get; set; }

		[DisplayName("报名结束时间")]
		[GreaterThanAttribute("RegistBeginDate")]
		public System.DateTime RegistEndDate { get; set; }

		[DisplayName("活动费用")]
		public decimal Price { get; set; }

		[DisplayName("活动开始时间")]
		[Required]
		public System.DateTime StartDate { get; set; }

		[DisplayName("活动天数")]
		[Required]
		public int Days { get; set; }
	}

}