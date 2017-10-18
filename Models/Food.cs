using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Models
{
    public class Food : IConsumable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public FoodType Type { get; set; }
        public string Unit { get; set; }
        public bool IsItemized { get; set; }

        public double Calories { get; set; }
        public Fats Fat { get; set; }
        public double Cholesterol { get; set; }
        public double Sodium { get; set; }
        public Carbohydrates Carbohydrate { get; set; }
        public double Protein { get; set; }
        public VitaminList Vitamins { get; set; }
    }
}
