using Subnautica.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Subnautica.Models
{
	public class MainViewModel
	{
		public Guid MaterialID { get; set; }
		public Guid? MaterialProductID { get; set; }
		public String MaterialName { get; set; }
		public String MaterialTypeName { get; set; }
		public String InstrumentName { get; set; }
		public Int32? MaterialQuantitySet { get; set; }
		public Int32? MaterialQuantity { get; set; }
		public List<SelectListItem> Materials { get; set; }
		public Guid MaterialTypeID { get; set; }
		public List<SelectListItem> MaterialTypes { get; set; }
		public Guid InstrumentID { get; set; }
		public List<SelectListItem> Instruments { get; set; }

		public List<ResourceListViewModel> ResourceListViewModel { get; set; }

		public MaterialType MaterialType { get; set; }
		public InstrumentType InstrumentType { get; set; }

	}
}