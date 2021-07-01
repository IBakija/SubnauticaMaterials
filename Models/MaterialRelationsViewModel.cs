using Subnautica.Enumerations;
using Subnautica.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Subnautica.Models
{
	public class MaterialRelationsViewModel
	{
		public Guid? ID { get; set; }
		//public Guid ResourceID { get; set; }
		//public String ResourceName { get; set; }
		//public Int32 ResourceQuantity { get; set; }
		public String ProductName { get; set; }
		public Guid? ProductID { get; set; }
		public Int32 ProductQuantity { get; set; }

		public List<SelectListItem> Products { get; set; }

		public IEnumerable<Material> Resources { get; set; }
		public IEnumerable<MaterialRelation> Relations { get; set; }
		public List<ResourceListViewModel> ResourceListViewModel { get; set; }

		public Enumerations.InstrumentType InstrumentType { get; set; }
	}
}