using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Subnautica.Enumerations
{
	public enum MaterialType
	{
		Raw = 1,
		Basic = 2,
		Advanced = 3,
		Electronics = 4,
		Water = 5,
		[Display(Name = "Cooked Food")]
		CookedFood = 6,
		[Display(Name = "Salted Food")]
		SaltedFood = 7,
		Equipment = 8,
		Tools = 9,
		Deployables = 10,
		[Display(Name = "Base Pieces")]
		BasePieces = 11,
		[Display(Name = "Exterior Modules")]
		ExteriorModules = 12,
		[Display(Name = "Interior Pieces")]
		InteriorPieces = 13,
		[Display(Name = "Interior Modules")]
		InteriorModules = 14,
		[Display(Name = "Miscellaneous Items")]
		MiscellaneousItems = 15,
		Vehicles = 16,
		[Display(Name = "Neptune Escape Rocket")]
		NeptuneEscapeRocket = 17
	}
}