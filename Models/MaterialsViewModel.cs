using Subnautica.SQLData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Subnautica.Models
{
	public class MaterialsViewModel
	{
		public Guid? ID { get; set; }
		public String Name { get; set; }
		[DataType(DataType.MultilineText)]
		public String Description { get; set; }
		public String Location { get; set; }
		public String Medium { get; set; }
		public String ImageURL { get; set; }
		public Int32 Size { get; set; }
		public String FilterType { get;set; }

		public Enumerations.MaterialType MaterialTypes { get; set; }
		public Enumerations.MaterialType MaterialTypesList { get; set; }
		public IEnumerable<Material> Materials { get; set; }
	}
}