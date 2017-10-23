using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.Models.SaveModels;
using DietPlanner.ViewModels.Common;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Recipies
{
    public class RecipiesViewModel : BindableBase
    {
        private string filePathMeal = "Data/Meals.json";
        private MainViewModel mainViewModel;
        public ObservableCollection<RecipeViewModel> Recipes { get; set; }
        

        public RecipiesViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;

            Recipes = new ObservableCollection<RecipeViewModel>();
            LoadRecipes();

            Recipes.CollectionChanged += (sender, args) =>
            {
                // Save when the collection is changed
                SaveRecipes();
            };
        }

        private EditRecipeViewModel editingRecipe;
        public EditRecipeViewModel EditingRecipe
        {
            get { return editingRecipe; }
            set
            {
                SetProperty(ref editingRecipe, value);
                ShowEditing = value != null;
            }
        }

        private RecipeViewModel _selectedRecipe;
        public RecipeViewModel SelectedRecipe
        {
            get { return _selectedRecipe; }
            set { SetProperty(ref _selectedRecipe, value); }
        }

        private bool _showEditing;
        public bool ShowEditing
        {
            get { return _showEditing; }
            set
            {
                SetProperty(ref _showEditing, value);
                // Update the title
                OnPropertyChanged(nameof(EditingTitle));
            }
        }

        public string EditingTitle
        {
            get
            {
                if (EditingRecipe == null)
                    return "";
                if (String.IsNullOrWhiteSpace(EditingRecipe.Name))
                    return "Create new food";
                return $"Edit {EditingRecipe.Name}";
            }
        }

        private ICommand _create;
        public ICommand Create
        {
            get
            {
                if (_create == null)
                {
                    _create = new DelegateCommand(() =>
                    {
                        EditingRecipe = new EditRecipeViewModel(mainViewModel, null, SaveRecipeUpdate);
                    });
                }
                return _create;
            }
        }

        private ICommand _editSelectedRecipe;
        public ICommand EditSelectedRecipe
        {
            get
            {
                if (_editSelectedRecipe == null)
                {
                    _editSelectedRecipe = new DelegateCommand(() =>
                    {
                        if (SelectedRecipe == null)
                            return;
                        EditingRecipe = new EditRecipeViewModel(mainViewModel, SelectedRecipe, SaveRecipeUpdate);
                    });
                }
                return _editSelectedRecipe;
            }
        }

        private void SaveRecipeUpdate(RecipeViewModel x)
        {
            EditingRecipe = null;
            // new food item
            if (x.Id == null)
            {
                x.Id = Guid.NewGuid().ToString();
                Recipes.Add(x);
            }
            else
            {
                // Update existing food item
                var meal = Recipes.FirstOrDefault(y => y.Id == x.Id);
                if (meal == null)
                {
                    Recipes.Add(x);
                }
                else
                {
                    meal.Clone(x);
                    // We save when we change the collection, so this is the only place we need to call save
                    SaveRecipes();
                }
            }
        }

        private void SaveRecipes()
        {
            List<MealSaveModel> meals = new List<MealSaveModel>();
            foreach (RecipeViewModel meal in Recipes)
            {
                var saveModel = new MealSaveModel()
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Items = new List<MealSaveModel.MealItemSaveModel>()
                };
                foreach (RecipeItemViewModel item in meal.FoodItems)
                {
                    saveModel.Items.Add(new MealSaveModel.MealItemSaveModel()
                    {
                        FoodId = item.Consumable.Id,
                        Quantity = item.Quantity
                    });
                }
                meals.Add(saveModel);
            }

            var json = JsonConvert.SerializeObject(meals);
            File.WriteAllText(filePathMeal, json, Encoding.UTF8);
        }

        private void LoadRecipes()
        {
            if (!File.Exists(filePathMeal))
            {
                return;
            }

            Recipes.Clear();

            var conten = File.ReadAllText(filePathMeal, Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<MealSaveModel>>(conten);

            Dictionary<String, ConsumableViewModel> foods = new Dictionary<string, ConsumableViewModel>();
            foreach (ConsumableViewModel x in mainViewModel.Consumables.Consumables)
            {
                foods.Add(x.Id, x);
            }

            foreach (MealSaveModel mealSaveModel in list)
            {
                RecipeViewModel mealViewModel = new RecipeViewModel()
                {
                    Id = mealSaveModel.Id,
                    Name = mealSaveModel.Name
                };
                foreach (MealSaveModel.MealItemSaveModel mealItemSaveModel in mealSaveModel.Items)
                {
                    if (!foods.ContainsKey(mealItemSaveModel.FoodId))
                    {
                        continue;
                    }
                    var food = foods[mealItemSaveModel.FoodId];
                    mealViewModel.FoodItems.Add(new RecipeItemViewModel()
                    {
                        Consumable = food,
                        Quantity = mealItemSaveModel.Quantity
                    });
                }
                if (mealViewModel.FoodItems.Count > 0)
                {
                    Recipes.Add(mealViewModel);
                }
            }
        }
    }
}
