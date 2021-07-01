using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Subnautica.Models
{
	public class ResourceListViewModel
	{
		public Guid ID { get; set; }
		public String Name { get; set; }
		public String InstrumentName { get; set; }
		public String MaterialTypeName { get; set; }
		public Int32 Quantity { get; set; }

		public List<SelectListItem> Materials { get; set; }
	}
}