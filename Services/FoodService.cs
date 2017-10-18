using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;
using DietPlanner.Models;
using Newtonsoft.Json;

namespace DietPlanner.Services
{
    public class FoodService
    {
        private static string fileFoods = "Data/Foods.json";
        private static string fileMeals = "Data/Meals.json";
        private List<Food> foods = new List<Food>()
        {
            new Food()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Ground Turkey",
                Unit = "g",
                Type = FoodType.Poultry,
                IsItemized = false,
                Fat = new Fats()
                {
                    Total = 8.0 / 112.0,
                    Saturated = 2.5 / 112.0,
                    Trans = 0
                },
                Cholesterol = 80.0 / 112.0,
                Sodium = 80.0 / 112.0,
                Carbohydrate = new Carbohydrates()
                {
                    Total = 0,
                    DietaryFiber = 0,
                    Sugar = 0
                },
                Protein = 21.0 / 112.0
            },
            new Food()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Whole Wheat bread",
                Unit = "Slice",
                Type = FoodType.Bread,
                IsItemized = true,
                Fat = new Fats()
                {
                    Total = 1,
                    Saturated = 0.5,
                    Trans = 0,
                    Polyunsaturated = 0.5,
                    Monounsaturated = 0,
                },
                Cholesterol = 0,
                Sodium = 200,
                Carbohydrate = new Carbohydrates()
                {
                    Total = 19,
                    DietaryFiber = 3,
                    Sugar = 3
                },
                Protein = 5
            }
        };
        private List<Meal> meals = new List<Meal>();

        public FoodService()
        {
            // Temp
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            if (!File.Exists(fileFoods))
            {
                var json = JsonConvert.SerializeObject(foods);
                File.WriteAllText(fileFoods, json, Encoding.UTF8);
            }

            // Currently we want to use the static
            //Load();
        }

        private void Load()
        {
            LoadFoods();
            LoadMeals();
        }

        private void LoadFoods()
        {
            if (!File.Exists(fileFoods))
            {
                foods = new List<Food>();
                return;
            }
            foods = JsonConvert.DeserializeObject<List<Food>>(File.ReadAllText(fileFoods));
        }
        
        public void LoadMeals()
        {
            if (!File.Exists(fileMeals))
            {
                meals = new List<Meal>();
                return;
            }

            Dictionary<String, Food> foodMap = new Dictionary<string, Food>();
            foreach (var f in foods)
            {
                foodMap.Add(f.Id, f);
            }
            List<Meal> mealList = new List<Meal>();
            var mealsSave = JsonConvert.DeserializeObject<List<MealSaveModel>>(File.ReadAllText(fileMeals));
            foreach (MealSaveModel meal in mealsSave)
            {
                Meal m = new Meal()
                {
                    Id = meal.Id,
                    Name = meal.Name
                };
                foreach (MealSaveModel.MealSaveModelItem item in meal.FoodItems)
                {
                    if (foodMap.ContainsKey(item.FoodId))
                    {
                        m.MealItems.Add(new Meal.MealItem()
                        {
                            Food = foodMap[item.FoodId],
                            Units = item.Units
                        });
                    }
                }
            }

            meals = mealList;
        }

        public List<Food> GetAll()
        {
            return foods;
        }

        public void Save(Food food)
        {
            if (food.Id != null)
            {
                var remove = foods.FirstOrDefault(x => x.Id == food.Id);
                if (remove != null)
                    foods.Remove(remove);
            }
            else
            {
                food.Id = Guid.NewGuid().ToString();
            }
            foods.Add(food);

            var json = JsonConvert.SerializeObject(foods);
            File.WriteAllText(fileFoods, json, Encoding.UTF8);
        }

        public void SaveMeal(Meal meal)
        {
            if (meal.Id != null)
            {
                var remove = meals.FirstOrDefault(x => x.Id == meal.Id);
                if (remove != null)
                    meals.Remove(remove);
            }
            else
            {
                meal.Id = Guid.NewGuid().ToString();
            }
            meals.Add(meal);

            List<MealSaveModel> saveModel = new List<MealSaveModel>();
            foreach (Meal m in meals)
            {
                saveModel.Add(new MealSaveModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    FoodItems = m.MealItems.Select(x => new MealSaveModel.MealSaveModelItem()
                    {
                        FoodId = x.Food.Id,
                        Units = x.Units
                    }).ToList()
                });
            }

            var json = JsonConvert.SerializeObject(saveModel);
            File.WriteAllText(fileMeals, json, Encoding.UTF8);
        }

        public List<Meal> GetAllMeals()
        {
            return meals;
        }

        public class MealSaveModel
        {
            public string Id;
            public string Name;
            public List<MealSaveModelItem> FoodItems;

            public class MealSaveModelItem
            {
                public String FoodId;
                public double Units;
            }
        }
    }
}
