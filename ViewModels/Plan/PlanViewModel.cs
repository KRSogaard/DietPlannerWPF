﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietPlanner.Models;
using DietPlanner.Models.SaveModels;
using DietPlanner.ViewModels.Common;
using DietPlanner.ViewModels.Plan;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class PlanViewModel : BindableBase
    {
        private string filePathPlan = "Data/Plan.json";
        public MainViewModel mainViewModel;
        private bool disableSave = false;

        public PlanDayViewModel Monday { get; set; }
        public PlanDayViewModel Tuesday { get; set; }
        public PlanDayViewModel Wednesday { get; set; }
        public PlanDayViewModel Thursday { get; set; }
        public PlanDayViewModel Friday { get; set; }
        public PlanDayViewModel Saturday { get; set; }
        public PlanDayViewModel Sunday { get; set; }

        public PlanViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;

            Monday = new PlanDayViewModel(mainViewModel, this);
            Tuesday = new PlanDayViewModel(mainViewModel, this);
            Wednesday = new PlanDayViewModel(mainViewModel, this);
            Thursday = new PlanDayViewModel(mainViewModel, this);
            Friday = new PlanDayViewModel(mainViewModel, this);
            Saturday = new PlanDayViewModel(mainViewModel, this);
            Sunday = new PlanDayViewModel(mainViewModel, this);

            LoadPlan();
        }

        public void SavePlan()
        {
            if (disableSave)
                return;

            var saveModel = PlanSaveModel.Convert(this);
            var json = JsonConvert.SerializeObject(saveModel);
            File.WriteAllText(filePathPlan, json, Encoding.UTF8);
        }

        public void LoadPlan()
        {
            disableSave = true;

            var conten = File.ReadAllText(filePathPlan, Encoding.UTF8);
            var saveModel = JsonConvert.DeserializeObject<PlanSaveModel>(conten);

            Dictionary<String, ConsumableViewModel> foods = new Dictionary<string, ConsumableViewModel>();
            foreach (ConsumableViewModel x in mainViewModel.Consumables.Consumables)
            {
                foods.Add(x.Id, x);
            }
            Dictionary<String, RecipeViewModel> recipes = new Dictionary<string, RecipeViewModel>();
            foreach (RecipeViewModel x in mainViewModel.Recipies.Recipes)
            {
                recipes.Add(x.Id, x);
            }

            loadDay(Monday, saveModel.Monday, foods, recipes);
            loadDay(Tuesday, saveModel.Tuesday, foods, recipes);
            loadDay(Wednesday, saveModel.Wednesday, foods, recipes);
            loadDay(Thursday, saveModel.Thursday, foods, recipes);
            loadDay(Friday, saveModel.Friday, foods, recipes);
            loadDay(Saturday, saveModel.Saturday, foods, recipes);
            loadDay(Sunday, saveModel.Sunday, foods, recipes);

            disableSave = false;
        }

        private void loadDay(PlanDayViewModel day, PlanDaySaveModel save,
            Dictionary<string, ConsumableViewModel> foods, 
            Dictionary<string, RecipeViewModel> recipes)
        {
            day.Meals.Clear();

            if (save == null)
                return;

            foreach (PlanMealSaveModel saveMeal in save.Meals)
            {
                PlanMealViewModel model = new PlanMealViewModel(mainViewModel, this)
                {
                    Name = saveMeal.Name,
                    Hour = saveMeal.Hour,
                    Min = saveMeal.Min
                };
                foreach (PlanItemSaveModel item in saveMeal.Items)
                {
                    IConsumableViewModel consumable = null;
                    if (item.IsRecipe)
                    {
                        if (recipes.ContainsKey(item.Id))
                        {
                            consumable = recipes[item.Id];
                        }
                    }
                    else
                    {
                        if (foods.ContainsKey(item.Id))
                        {
                            consumable = foods[item.Id];
                        }
                    }
                    if (consumable == null)
                    {
                        continue;
                    }
                    model.Consumables.Add(new PlanConsumableViewModel(consumable)
                    {
                        Quantity = item.Quantity
                    });
                }
                day.Meals.Add(model);
            }
        }
    }
}