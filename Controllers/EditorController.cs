using Subnautica.Models;
using Subnautica.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Subnautica.Controllers
{
	public class EditorController : Controller
	{
		[HttpGet]
		public ActionResult Materials(Guid? id)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			IEnumerable<MaterialType> materialTypes = subnauticaEntities.MaterialType.OrderBy(x => x.Name).ToList();

			MaterialsViewModel model = new MaterialsViewModel();
			if (id != null)
			{
				Material material = subnauticaEntities.Material.Where(x => x.ID == id).FirstOrDefault();
				model.ID = material.ID;
				model.Name = material.Name;
				model.Description = material.Description;
				model.Size = material.Size ?? 1;

				MaterialType materialType = subnauticaEntities.MaterialType.Where(x => x.ID == material.TypeID).FirstOrDefault();

				Enumerations.MaterialType mat = (Enumerations.MaterialType)Enum.Parse(typeof(Enumerations.MaterialType), materialType.Name.Replace(" ", ""));

				model.MaterialTypes = mat;
			}

			subnauticaEntities.Dispose();
			return View(model);
		}

		[HttpPost]
		public ActionResult Materials(MaterialsViewModel model)
		{
			if ((Int32)model.MaterialTypes == 0)
			{
				ModelState.AddModelError("MaterialTypes", "You have to choose a type of Material.");
				return View(model);
			}

			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			Material material = subnauticaEntities.Material.FirstOrDefault(x => x.ID == model.ID);
			Boolean newMaterial = false;

			if (material == null)
			{
				Material materialCheck = subnauticaEntities.Material.FirstOrDefault(x => x.Name == model.Name.Trim());
				if (materialCheck != null)
				{
					ModelState.AddModelError("Duplicate", "A material with that name already exists!");
					return View(model);
				}

				newMaterial = true;
				material = new Material();
				material.ID = Guid.NewGuid();
			}

			material.Name = model.Name.Trim();
			material.Description = model.Description;
			material.ImageURL = model.ImageURL;
			material.Size = model.Size;
			material.TypeID = subnauticaEntities.MaterialType.Where(x => x.Enum == (Int32)model.MaterialTypes).Select(x => x.ID).FirstOrDefault();

			if (newMaterial)
			{
				subnauticaEntities.Material.Add(material);
			}
			else
			{
				subnauticaEntities.Entry(material).State = System.Data.Entity.EntityState.Modified;
			}
			subnauticaEntities.SaveChanges();

			ModelState.Clear();

			subnauticaEntities.Dispose();
			return View(new MaterialsViewModel());
		}

		public ActionResult _MaterialList(String val, String typeVal, MaterialsViewModel materialsViewModel)
		{
			Int32 type = 0;
			if (typeVal != "0")
			{
				type = Convert.ToInt32(typeVal);
			}

			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<Material> materials = subnauticaEntities.Material
				.Where(x => (
				(typeVal != "0" && !String.IsNullOrEmpty(typeVal)) ? x.MaterialType.Enum == type : true) &&
				((!String.IsNullOrEmpty(val)) ? x.Name.Contains(val) : true) &&
					(!String.IsNullOrEmpty(materialsViewModel.FilterType) ? x.MaterialType.Name == materialsViewModel.FilterType : true))
				.OrderBy(x => x.Name)
				.ToList();
			MaterialsViewModel model = new MaterialsViewModel()
			{
				Materials = materials
			};

			return PartialView(model);
		}

		[HttpGet]
		public ActionResult MaterialTypes(Guid? id)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			IEnumerable<Instrument> instruments = subnauticaEntities.Instrument.ToList();

			MaterialTypesViewModel model = new MaterialTypesViewModel();
			if (id != null)
			{
				MaterialType materialType = subnauticaEntities.MaterialType.Where(x => x.ID == id).FirstOrDefault();
				model.ID = materialType.ID;
				model.Name = materialType.Name;
				model.Description = materialType.Description;

				Instrument instrumentType = subnauticaEntities.Instrument.Where(x => x.ID == materialType.InstrumentID).FirstOrDefault();

				Enumerations.InstrumentType mat = (Enumerations.InstrumentType)Enum.Parse(typeof(Enumerations.InstrumentType), instrumentType.Name.Replace(" ", ""));

				model.InstrumentType = mat;
			}

			subnauticaEntities.Dispose();
			return View(model);
		}

		[HttpPost]
		public ActionResult MaterialTypes(MaterialTypesViewModel model)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			MaterialType materialType = subnauticaEntities.MaterialType.FirstOrDefault(x => x.ID == model.ID);
			Boolean newMaterialType = false;

			if (materialType == null)
			{
				MaterialType materialTypeCheck = subnauticaEntities.MaterialType.FirstOrDefault(x => x.Name == model.Name.Trim());
				if (materialTypeCheck != null)
				{
					ModelState.AddModelError("Duplicate", "A material with that name already exists!");
					return View(model);
				}
				newMaterialType = true;
				materialType = new MaterialType();
				materialType.ID = Guid.NewGuid();
			}

			Int32 highestEnum = subnauticaEntities.MaterialType.Max(x => x.Enum);

			materialType.Name = model.Name.Trim();
			materialType.Description = model.Description;
			materialType.InstrumentID = subnauticaEntities.Instrument.Where(x => x.Enum == (Int32)model.InstrumentType).Select(x => x.ID).FirstOrDefault();

			if (newMaterialType)
			{
				materialType.Enum = (highestEnum + 1);
				subnauticaEntities.MaterialType.Add(materialType);
			}
			else
			{
				subnauticaEntities.Entry(materialType).State = System.Data.Entity.EntityState.Modified;
			}
			subnauticaEntities.SaveChanges();

			ModelState.Clear();

			subnauticaEntities.Dispose();
			return View();
		}
		public ActionResult _MaterialTypeList(String val, String typeVal)
		{
			Int32 type = 0;
			if (typeVal != "0")
			{
				type = Convert.ToInt32(typeVal);
			}

			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<MaterialType> materials = subnauticaEntities.MaterialType
				.Where(x => ((typeVal != "0" && !String.IsNullOrEmpty(typeVal)) ? x.Instrument.Enum == type : true) &&
					((!String.IsNullOrEmpty(val)) ? x.Name.Contains(val) : true) &&
					(type == 999 ? x.InstrumentID == null : true))
				.OrderBy(x => x.Name)
				.ToList();
			MaterialTypesViewModel model = new MaterialTypesViewModel()
			{
				MaterialTypesList = materials
			};

			return PartialView(model);
		}

		[HttpGet]
		public ActionResult MaterialRelations(Guid? id)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<Material> materials = subnauticaEntities.Material.ToList();

			MaterialRelationsViewModel model = new MaterialRelationsViewModel()
			{
				ProductQuantity = 1,
			};

			model.ResourceListViewModel = new List<ResourceListViewModel>();



			if (id != null)
			{
				MaterialRelation materialRelation = subnauticaEntities.MaterialRelation.FirstOrDefault(x => x.ID == id);

				model.ProductID = materialRelation.ResourceID;
				model.ProductName = materialRelation.Material.Name;
				model.ProductQuantity = materialRelation.Quantity;

				List<MaterialRelation> materialRelations = subnauticaEntities.MaterialRelation.Where(x => x.ProductID == materialRelation.ID).ToList();

				for (var i = 0; i < materialRelations.Count(); i++)
				{
					List<SelectListItem> materialsList = materials
				.OrderBy(x => x.Name)
				.Select(s => new SelectListItem
				{
					Text = s.Name,
					Value = s.ID.ToString()
				})
				.ToList();
					model.ResourceListViewModel.Add(new ResourceListViewModel());
					model.ResourceListViewModel[i].ID = materialRelations[i].ResourceID;
					model.ResourceListViewModel[i].Name = materialRelations[i].Material.Name;
					model.ResourceListViewModel[i].Quantity = materialRelations[i].Quantity;
					model.ResourceListViewModel[i].Materials = materialsList;

					var selected = model.ResourceListViewModel[i].Materials.Where(x => x.Value == model.ResourceListViewModel[i].ID.ToString()).First();
					selected.Selected = true;
				}
			}
			else
			{
				List<SelectListItem> materialsList = materials
				.OrderBy(x => x.Name)
				.Select(s => new SelectListItem
				{
					Text = s.Name,
					Value = s.ID.ToString()
				})
				.ToList();

				// List  of all product guids
				List<Guid> MRguids = subnauticaEntities.MaterialRelation.Where(x => x.ProductID == null).Select(x => x.ResourceID).ToList();
				foreach (var guid in MRguids)
				{
					Material material = subnauticaEntities.Material.Where(x => x.ID == guid).FirstOrDefault();
					materials.Remove(material);
				}

				model.ResourceListViewModel.Add(new ResourceListViewModel());
				model.ResourceListViewModel[0].Materials = materialsList;
				model.ResourceListViewModel[0].Quantity = 1;
			}

			List<SelectListItem> productsList = materials
				.Where(x => x.MaterialType.Enum != (Int32)Enumerations.MaterialType.Raw)
				.OrderBy(x => x.Name)
				.Select(s => new SelectListItem
				{
					Text = s.Name,
					Value = s.ID.ToString()
				})
				.ToList();

			model.Products = productsList;

			subnauticaEntities.Dispose();
			return View(model);
		}

		[HttpPost]
		public ActionResult MaterialRelations(MaterialRelationsViewModel model)
		{
			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			//MaterialRelation relation = subnauticaEntities.MaterialRelation.Where(x => x.ResourceID == model.ProductID && x.ProductID == null).FirstOrDefault();
			MaterialRelation relation = subnauticaEntities.MaterialRelation.Where(x => x.ID == model.ID).FirstOrDefault();

			MaterialRelation materialRelationsProduct;

			if (relation != null)
			{
				materialRelationsProduct = relation;
				materialRelationsProduct.ResourceID = model.ProductID ?? Guid.Empty;
				materialRelationsProduct.Quantity = model.ProductQuantity;

				Guid materialRelationsID = subnauticaEntities.MaterialRelation.Where(x => x.ResourceID == model.ProductID && x.ProductID == null).Select(x => x.ID).FirstOrDefault();

				subnauticaEntities.Entry(materialRelationsProduct).State = System.Data.Entity.EntityState.Modified;

				List<MaterialRelation> relations = subnauticaEntities.MaterialRelation.Where(x => x.ProductID == relation.ID).ToList();

				foreach (var item in relations)
				{
					subnauticaEntities.Entry(item).State = System.Data.Entity.EntityState.Deleted;
				}

				for (var i = 0; i < model.ResourceListViewModel.Count(); i++)
				{
					MaterialRelation resource = subnauticaEntities.MaterialRelation.Where(x => x.ResourceID == model.ResourceListViewModel[i].ID && x.ProductID == relation.ID).FirstOrDefault();

					resource.ResourceID = model.ResourceListViewModel[i].ID;
					resource.Quantity = model.ResourceListViewModel[i].Quantity;

					resource.ProductID = materialRelationsID;

					subnauticaEntities.Entry(resource).State = System.Data.Entity.EntityState.Modified;
					subnauticaEntities.SaveChanges();
				}
			}
			else
			{
				materialRelationsProduct = new MaterialRelation()
				{
					ID = Guid.NewGuid(),
					ResourceID = model.ProductID ?? Guid.Empty,
					Quantity = model.ProductQuantity,
				};

				subnauticaEntities.MaterialRelation.Add(materialRelationsProduct);
				subnauticaEntities.SaveChanges();

				Guid materialRelationsID = subnauticaEntities.MaterialRelation.Where(x => x.ResourceID == model.ProductID && x.ProductID == null).Select(x => x.ID).FirstOrDefault();

				for (var i = 0; i < model.ResourceListViewModel.Count(); i++)
				{
					MaterialRelation materialRelationsResource = new MaterialRelation()
					{
						ID = Guid.NewGuid(),
						ResourceID = model.ResourceListViewModel[i].ID,
						Quantity = model.ResourceListViewModel[i].Quantity,

						ProductID = materialRelationsID
					};

					subnauticaEntities.MaterialRelation.Add(materialRelationsResource);
					subnauticaEntities.SaveChanges();
				}
			}

			subnauticaEntities.SaveChanges();

			ModelState.Clear();

			List<Material> materials = subnauticaEntities.Material.ToList();

			List<SelectListItem> materialsList = materials
				.OrderBy(x => x.Name)
				.Select(s => new SelectListItem
				{
					Text = s.Name,
					Value = s.ID.ToString()
				})
				.ToList();

			// List  of all product guids
			List<Guid> MRguids = subnauticaEntities.MaterialRelation.Where(x => x.ProductID == null).Select(x => x.ResourceID).ToList();

			foreach (var guid in MRguids)
			{
				Material material = subnauticaEntities.Material.Where(x => x.ID == guid).FirstOrDefault();
				materials.Remove(material);
			}

			List<SelectListItem> productsList = materials
				.Where(x => x.MaterialType.Enum != (Int32)Enumerations.MaterialType.Raw)
				.OrderBy(x => x.Name)
				.Select(s => new SelectListItem
				{
					Text = s.Name,
					Value = s.ID.ToString()
				})
				.ToList();

			model.ResourceListViewModel = new List<ResourceListViewModel>();
			model.ResourceListViewModel.Add(new ResourceListViewModel());
			model.ResourceListViewModel[0].Materials = materialsList;
			model.ResourceListViewModel[0].Quantity = 1;
			model.Products = productsList;

			subnauticaEntities.Dispose();
			return View(model);
		}

		public ActionResult _MaterialRelationList(String val, String typeVal)
		{
			Int32 type = 0;
			if (typeVal != "0")
			{
				type = Convert.ToInt32(typeVal);
			}

			SubnauticaEntities subnauticaEntities = new SubnauticaEntities();

			List<MaterialRelation> relations = subnauticaEntities.MaterialRelation
				.Where(x => x.ProductID == null &&
					((typeVal != "0" && !String.IsNullOrEmpty(typeVal)) ? x.Material.MaterialType.Instrument.Enum == type : true) &&
					((!String.IsNullOrEmpty(val)) ? x.Material.Name.Contains(val) : true) &&
					(type == 999 ? x.Material.MaterialType.InstrumentID == null : true))
				.OrderBy(x => x.Material.Name).ToList();


			List<ResourceListViewModel> resources = new List<ResourceListViewModel>();

			foreach (MaterialRelation relation in relations)
			{
				resources.Add(new ResourceListViewModel
				{
					ID = relation.ID,
					InstrumentName = relation.Material.MaterialType.Instrument.Name,
					Name = relation.Material.Name,
					Quantity = relation.Quantity
				});
			};

			MaterialRelationsViewModel model = new MaterialRelationsViewModel()
			{
				ResourceListViewModel = resources
			};

			return PartialView(model);
		}
	}
}