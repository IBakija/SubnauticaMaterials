using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Subnautica.Enumerations
{
	public enum InstrumentType
	{
		Fabricator = 1,
		[Display(Name = "Habitat Builder")]
		HabitatBuilder = 2,
		[Display(Name = "Mobile Vehicle Bay")]
		MobileVehicleBay = 3,
		[Display(Name = "Vehicle Upgrade Console")]
		VehicleUpgradeConsole = 4,
		[Display(Name = "Modification Station")]
		ModificationStation = 5,
		[Display(Name = "Cyclops' Upgrade Fabricator")]
		CyclopsUpgradeFabricator = 6,
		[Display(Name = "Scanner Room's Fabricator")]
		ScannerRoomFabricator = 7,
		[Display(Name = "Neptune Launch Platform")]
		NeptuneLaunchPlatform = 8,
		None = 999
	}
}