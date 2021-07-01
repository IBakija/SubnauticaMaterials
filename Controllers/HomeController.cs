using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Subnautica.Models;
using Subnautica.SQLData;

namespace Subnautica.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<Instrument> instruments = subnauticaEntities.Instrument.ToList();

			MainViewModel model = new MainViewModel()
			{
				Instruments = instruments
							.OrderBy(x => x.Name)
							.Select(s => new SelectListItem
							{
								Text = s.Name,
								Value = s.ID.ToString()
							})
							.ToList(),
			};

			model.Instruments.Insert(0, new SelectListItem
			{
				Text = "-",
				Value = String.Empty,
				Selected = true
			});

			return View(model);
		}

		public ActionResult _ProductList(String val, Int32? quantity)
		{
			MainViewModel model = new MainViewModel();

			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			if (quantity < 1)
			{
				quantity = 1;
			}

			if (String.IsNullOrEmpty(val))
			{
				val = Guid.Empty.ToString();
			}
			else
			{
				Guid value = Guid.Parse(val);
				MaterialRelation relation = subnauticaEntities.MaterialRelation
				   .Where(x => x.ResourceID == value && x.ProductID == null)
				   .FirstOrDefault();

				model.MaterialProductID = relation.ResourceID;
				model.MaterialName = relation.Material.Name;
				model.MaterialQuantitySet = (quantity ?? 1);
				model.MaterialQuantity = relation.Quantity * (quantity ?? 1);
				model.MaterialTypeName = relation.Material.MaterialType.Name;
				model.InstrumentName = relation.Material.MaterialType.Instrument != null ? relation.Material.MaterialType.Instrument.Name : "";
			}

			return PartialView(model);
		}

		public ActionResult _MaterialList(String val, Int32? quantity, List<Guid> idList, List<Int32> quantityList, Boolean isPlus = false, Boolean isMinus = false)
		{
			MainViewModel model = new MainViewModel();

			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			if (String.IsNullOrEmpty(val))
			{
				val = Guid.Empty.ToString();
			}

			Guid value = Guid.Parse(val);

			// Find new Product
			MaterialRelation product = subnauticaEntities.MaterialRelation
				.Where(x => x.ResourceID == value && x.ProductID == null)
				.FirstOrDefault();

			// Find all new products`s components
			List<MaterialRelation> components = new List<MaterialRelation>();
			if (product != null)
			{
				components = subnauticaEntities.MaterialRelation
					.Where(x => x.ProductID != null)
					.Where(x => x.ProductID == product.ID)
					.OrderBy(x => x.Material.Name)
					.ToList();
			}

			if (quantity == null)
			{
				quantity = 1;
			}

			model.ResourceListViewModel = new List<ResourceListViewModel>();

			// Addition or substruction of existing products and it`s components
			if (isMinus || isPlus)
			{
				// if it`s not the first product find all components of previous products
				if (idList != null)
				{
					for (var k = 0; k < idList.Count(); k++)
					{
						Guid componentsID = idList[k];
						Material material = subnauticaEntities.Material.FirstOrDefault(x => x.ID == componentsID);

						material.MaterialType = subnauticaEntities.MaterialType.Where(x => x.ID == material.TypeID).FirstOrDefault();

						material.MaterialType.Instrument = subnauticaEntities.Instrument.Where(x => x.ID == material.MaterialType.InstrumentID).FirstOrDefault();

						model.ResourceListViewModel.Add(new ResourceListViewModel());
						model.ResourceListViewModel[k].ID = material.ID;
						model.ResourceListViewModel[k].Name = material.Name;
						model.ResourceListViewModel[k].MaterialTypeName = material.MaterialType.Name;
						model.ResourceListViewModel[k].InstrumentName = material.MaterialType.Instrument != null ? material.MaterialType.Instrument.Name : "";

						MaterialRelation component = subnauticaEntities.MaterialRelation.Where(x => x.ProductID != null && x.ResourceID == componentsID)
																						.Where(x => x.ProductID == product.ID)
																						.FirstOrDefault();

						if (component != null)
						{
							quantityList[k] = isMinus ? quantityList[k] - component.Quantity : quantityList[k] + component.Quantity;
						}

						model.ResourceListViewModel[k].Quantity = quantityList[k];
					}

					// Remove from list if quantity is zero
					if (isMinus)
					{
						var count = idList.Count();

						for (var k = 0; k < count; k++)
						{
							if (model.ResourceListViewModel[k].Quantity == 0)
							{
								model.ResourceListViewModel.RemoveAt(k);
								count -= 1;
								k -= 1;
							}
						}
					}

				}

				return PartialView(model);
			}

			var j = 0;

			// if it`s not the first product find all components of previous products
			if (idList != null)
			{
				for (j = 0; j < idList.Count(); j++)
				{
					Guid componentsID = idList[j];
					Material material = subnauticaEntities.Material.FirstOrDefault(x => x.ID == componentsID);

					material.MaterialType = subnauticaEntities.MaterialType.Where(x => x.ID == material.TypeID).FirstOrDefault();

					material.MaterialType.Instrument = subnauticaEntities.Instrument.Where(x => x.ID == material.MaterialType.InstrumentID).FirstOrDefault();

					model.ResourceListViewModel.Add(new ResourceListViewModel());
					model.ResourceListViewModel[j].ID = material.ID;
					model.ResourceListViewModel[j].Name = material.Name;
					model.ResourceListViewModel[j].Quantity = quantityList[j];
					model.ResourceListViewModel[j].MaterialTypeName = material.MaterialType.Name;
					model.ResourceListViewModel[j].InstrumentName = material.MaterialType.Instrument != null ? material.MaterialType.Instrument.Name : "";
				}
			}

			for (var i = 0; i < components.Count(); i++)
			{
				Boolean inList = false;

				// One by one find component`s info
				MaterialRelation relationResource = components[i];
				components[i].Material = subnauticaEntities.Material.Where(x => x.ID == relationResource.ResourceID).FirstOrDefault();

				components[i].Material.MaterialType = subnauticaEntities.MaterialType.Where(x => x.ID == relationResource.Material.TypeID).FirstOrDefault();

				components[i].Material.MaterialType.Instrument = subnauticaEntities.Instrument.Where(x => x.ID == relationResource.Material.MaterialType.InstrumentID).FirstOrDefault();

				if (idList != null)
				{
					inList = idList.IndexOf(components[i].ResourceID) != -1;

					// Component exists already, add up quantity
					if (inList)
					{
						Int32 index = idList.IndexOf(components[i].ResourceID);
						quantityList[index] += (components[i].Quantity * (quantity ?? 1));

						model.ResourceListViewModel[index].Quantity = quantityList[index];
					}
					else // Component is new, add it to the list
					{
						model.ResourceListViewModel.Add(new ResourceListViewModel());
						model.ResourceListViewModel[j].ID = components[i].ResourceID;
						model.ResourceListViewModel[j].Name = components[i].Material.Name;
						model.ResourceListViewModel[j].Quantity = (components[i].Quantity * (quantity ?? 1));
						model.ResourceListViewModel[j].MaterialTypeName = components[i].Material.MaterialType.Name;
						model.ResourceListViewModel[j].InstrumentName = components[i].Material.MaterialType.Instrument != null ? components[i].Material.MaterialType.Instrument.Name : "";

						j++;
					}
				}
				else
				{
					model.ResourceListViewModel.Add(new ResourceListViewModel());
					model.ResourceListViewModel[i].ID = components[i].ResourceID;
					model.ResourceListViewModel[i].Name = components[i].Material.Name;
					model.ResourceListViewModel[i].Quantity = (components[i].Quantity * (quantity ?? 1));
					model.ResourceListViewModel[i].MaterialTypeName = components[i].Material.MaterialType.Name;
					model.ResourceListViewModel[i].InstrumentName = components[i].Material.MaterialType.Instrument != null ? components[i].Material.MaterialType.Instrument.Name : "";
				}
			}

			model.ResourceListViewModel.OrderBy(x => x.Name);

			return PartialView(model);
		}

		[HttpGet]
		public ActionResult _MaterialTypeContainer(Guid? instrumentID)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<MaterialType> materialTypes = subnauticaEntities.MaterialType
				.Where(x => (instrumentID != null ? x.InstrumentID == instrumentID : true))
				.ToList();

			MainViewModel model = new MainViewModel()
			{
				MaterialTypes = materialTypes
							.OrderBy(x => x.Name)
							.Select(s => new SelectListItem
							{
								Text = s.Name,
								Value = s.ID.ToString()
							})
							.ToList(),
			};

			model.MaterialTypes.Insert(0, new SelectListItem
			{
				Text = "-",
				Value = String.Empty,
				Selected = true
			});

			return PartialView(model);
		}

		[HttpGet]
		public ActionResult _MaterialContainer(Guid? instrumentID, Guid? materialTypeID)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<Material> materials = new List<Material>();

			List<Guid> MRguids = subnauticaEntities.MaterialRelation.Where(x => x.ProductID == null).Select(x => x.ResourceID).ToList();

			foreach (Guid guid in MRguids)
			{
				Material material = subnauticaEntities.Material
					.Where(x =>
						x.MaterialType.Enum != (Int32)Enumerations.MaterialType.Raw &&
						(instrumentID != null ? x.MaterialType.InstrumentID == instrumentID : true) &&
						(materialTypeID != null ? x.TypeID == materialTypeID : true) &&
						x.ID == guid)
					.FirstOrDefault();

				if (material != null)
				{
					materials.Add(material);
				}
			}

			MainViewModel model = new MainViewModel()
			{
				Materials = materials
							.OrderBy(x => x.Name)
							.Select(s => new SelectListItem
							{
								Text = s.Name,
								Value = s.ID.ToString()
							})
							.ToList()
			};

			model.Materials.Insert(0, new SelectListItem
			{
				Text = "-",
				Value = String.Empty,
				Selected = true
			});

			return PartialView(model);
		}

		[HttpGet]
		public ActionResult Products()
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<MaterialType> materialTypes = subnauticaEntities.MaterialType.ToList();

			MainViewModel model = new MainViewModel()
			{
				MaterialTypes = materialTypes
							.OrderBy(x => x.Name)
							.Select(s => new SelectListItem
							{
								Text = s.Name,
								Value = s.ID.ToString()
							})
							.ToList(),
			};

			model.MaterialTypes.Insert(0, new SelectListItem
			{
				Text = "-",
				Value = String.Empty,
				Selected = true
			});
			return View(model);
		}

		public ActionResult _IsUsedIn(String val)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			if (!String.IsNullOrEmpty(val))
			{
				Material material = subnauticaEntities.Material.FirstOrDefault(x => x.ID == Guid.Parse(val));

				List<MaterialRelation> product = subnauticaEntities.MaterialRelation.Where(x => x.ResourceID == material.ID && x.ProductID == null).ToList();
			}

			return PartialView();
		}

		public ActionResult _ComponentContainer(Guid? materialTypeID)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<Material> materials = new List<Material>();

			List<Guid> MRguids = subnauticaEntities.MaterialRelation.Where(x => x.ProductID != null).Select(x => x.ResourceID).Distinct().ToList();

			foreach (Guid guid in MRguids)
			{
				Material material = subnauticaEntities.Material
					.Where(x =>
						(materialTypeID != null ? x.TypeID == materialTypeID : true) &&
						x.ID == guid)
					.FirstOrDefault();

				if (material != null)
				{
					materials.Add(material);
				}
			}

			MainViewModel model = new MainViewModel()
			{
				Materials = materials
							.OrderBy(x => x.Name)
							.Select(s => new SelectListItem
							{
								Text = s.Name,
								Value = s.ID.ToString()
							})
							.ToList()
			};

			model.Materials.Insert(0, new SelectListItem
			{
				Text = "-",
				Value = String.Empty,
				Selected = true
			});

			return PartialView(model);
		}
	}
}