using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietPlanner.ViewModels;
using DietPlanner.ViewModels.Common;
using DietPlanner.ViewModels.Plan;

namespace DietPlanner.Models.SaveModels
{
    public class PlanSaveModel
    {
        public List<PlanDaySaveModel> Days { get; set; }

        public static PlanSaveModel Convert(PlanViewModel model)
        {
            return new PlanSaveModel()
            {
                Days = model.Days.Select(x => PlanDaySaveModel.Convert(x)).ToList()
            };
        }
    }

    public class PlanDaySaveModel
    {
        public String DayName { get; set; }
        public List<PlanMealSaveModel> Meals { get; set; }

        public PlanDaySaveModel()
        {
            Meals = new List<PlanMealSaveModel>();
        }

        public static PlanDaySaveModel Convert(PlanDayViewModel model)
        {
            var n = new PlanDaySaveModel()
            {
                DayName = model.DayName
            };
            foreach (PlanMealViewModel meal in model.Meals)
            {
                n.Meals.Add(PlanMealSaveModel.Convert(meal));
            }
            return n;
        }
    }

    public class PlanMealSaveModel
    {
        public string Name { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public List<PlanItemSaveModel> Items { get; set; }

        public PlanMealSaveModel()
        {
            Items = new List<PlanItemSaveModel>();
        }

        public static PlanMealSaveModel Convert(PlanMealViewModel meal)
        {
            var n = new PlanMealSaveModel()
            {
                Name = meal.Name,
                Hour = meal.Hour,
                Min = meal.Min
            };
            foreach (PlanConsumableViewModel item in meal.Consumables)
            {
                n.Items.Add(PlanItemSaveModel.Convert(item));
            }
            return n;
        }
    }

    public class PlanItemSaveModel
    {
        public bool IsRecipe { get; set; }
        public string Id { get; set; }
        public double Quantity { get; set; }

        public static PlanItemSaveModel Convert(PlanConsumableViewModel item)
        {
            return new PlanItemSaveModel()
            {
                Id = (item.Consumable is ConsumableViewModel) ? 
                            ((ConsumableViewModel)item.Consumable).Id : 
                            ((RecipeViewModel)item.Consumable).Id,
                IsRecipe = item.Consumable is RecipeViewModel,
                Quantity = item.Quantity
            };
    }
    }
}
