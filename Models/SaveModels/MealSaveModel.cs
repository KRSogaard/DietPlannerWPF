using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Models.SaveModels
{
    public class MealSaveModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<MealItemSaveModel> Items { get; set; }

        public MealSaveModel()
        {
            Items = new List<MealItemSaveModel>();
        }

        public class MealItemSaveModel
        {
            public string FoodId { get; set; }
            public double Quantity { get; set; }
        }
    }
}
