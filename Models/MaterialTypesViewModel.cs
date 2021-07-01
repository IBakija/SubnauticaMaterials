using Subnautica.SQLData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Subnautica.Models
{
	public class MaterialTypesViewModel
	{
		public Guid? ID { get; set; }
		public String Name { get; set; }
		[DataType(DataType.MultilineText)]
		public String Description { get; set; }
		public String Instrument { get; set; }

		public Enumerations.InstrumentType InstrumentType { get; set; }
		public Enumerations.InstrumentType InstrumentTypesList { get; set; }
		public IEnumerable<MaterialType> MaterialTypesList { get; set; }
	}
}