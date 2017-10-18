using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Models
{
    public class Meal : IConsumable
    {
        public List<MealItem> MealItems { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Unit => "meal";
        public FoodType Type => FoodType.Meal;

        public double Calories
        {
            get
            {
                double current = new double();
                foreach (MealItem mealItem in MealItems)
                {
                    current += mealItem.Food.Calories * mealItem.Units;
                }

                return current;
            }
        }
        public double Cholesterol
        {
            get
            {
                double current = new double();
                foreach (MealItem mealItem in MealItems)
                {
                    current += mealItem.Food.Cholesterol * mealItem.Units;
                }

                return current;
            }
        }
        public double Sodium
        {
            get
            {
                double current = new double();
                foreach (MealItem mealItem in MealItems)
                {
                    current += mealItem.Food.Sodium * mealItem.Units;
                }

                return current;
            }
        }
        public double Protein
        {
            get
            {
                double current = new double();
                foreach (MealItem mealItem in MealItems)
                {
                    current += mealItem.Food.Protein * mealItem.Units;
                }

                return current;
            }
        }
        public Fats Fat
        {
            get
            {
                Fats current = new Fats();
                foreach (MealItem mealItem in MealItems)
                {
                    current += mealItem.Food.Fat * mealItem.Units;
                }

                return current;
            }
        }
        public Carbohydrates Carbohydrate
        {
            get
            {
                Carbohydrates current = new Carbohydrates();
                foreach (MealItem mealItem in MealItems)
                {
                    current += mealItem.Food.Carbohydrate * mealItem.Units;
                }

                return current;
            }
        }
        public VitaminList Vitamins { get; }

        public Meal()
        {
            MealItems = new List<MealItem>();
        }

        public class MealItem
        {
            public Food Food;
            public double Units;
        }
    }
}
