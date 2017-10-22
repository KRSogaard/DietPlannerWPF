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
        public PlanDaySaveModel Monday { get; set; }
        public PlanDaySaveModel Tuesday { get; set; }
        public PlanDaySaveModel Wednesday { get; set; }
        public PlanDaySaveModel Thursday { get; set; }
        public PlanDaySaveModel Friday { get; set; }
        public PlanDaySaveModel Saturday { get; set; }
        public PlanDaySaveModel Sunday { get; set; }

        public static PlanSaveModel Convert(PlanViewModel model)
        {
            return new PlanSaveModel()
            {
                Monday = model.Monday == null ? null : PlanDaySaveModel.Convert(model.Monday),
                Tuesday = model.Tuesday == null ? null : PlanDaySaveModel.Convert(model.Tuesday),
                Wednesday = model.Wednesday == null ? null : PlanDaySaveModel.Convert(model.Wednesday),
                Thursday = model.Thursday == null ? null : PlanDaySaveModel.Convert(model.Thursday),
                Friday = model.Friday == null ? null : PlanDaySaveModel.Convert(model.Friday),
                Saturday = model.Saturday == null ? null : PlanDaySaveModel.Convert(model.Saturday),
                Sunday = model.Sunday == null ? null : PlanDaySaveModel.Convert(model.Sunday)
            };
        }
    }

    public class PlanDaySaveModel
    {
        public List<PlanMealSaveModel> Meals { get; set; }

        public PlanDaySaveModel()
        {
            Meals = new List<PlanMealSaveModel>();
        }

        public static PlanDaySaveModel Convert(PlanDayViewModel model)
        {
            var n = new PlanDaySaveModel();
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
