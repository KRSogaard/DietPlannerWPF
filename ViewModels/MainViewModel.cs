using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.Models;
using DietPlanner.Services;
using DietPlanner.ViewModels.Common;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string filePathFood = "Data/Foods.json";
        private FoodService foodService;

        public ObservableCollection<ConsumableViewModel> AllFoods { get; set; }

        #region Food
        private EditFoodViewModel _editFoodViewModel;
        public EditFoodViewModel EditFoodViewModel
        {
            get { return _editFoodViewModel; }
            set
            {
                SetProperty(ref _editFoodViewModel, value);
                ShowEditFood = value != null;
            }
        }

        private ConsumableViewModel _selectedFood;
        public ConsumableViewModel SelectedFood
        {
            get { return _selectedFood; }
            set { SetProperty(ref _selectedFood, value); }
        }

        private bool _showEditFood;
        public bool ShowEditFood
        {
            get { return _showEditFood; }
            set {
                SetProperty(ref _showEditFood, value);
                // Update the title
                OnPropertyChanged(nameof(EditFoodTitle));
            }
        }

        public string EditFoodTitle
        {
            get
            {
                if (EditFoodViewModel == null)
                    return "";
                if (String.IsNullOrWhiteSpace(EditFoodViewModel.Name))
                    return "Create new food";
                return $"Edit {EditFoodViewModel.Name}";
            }
        }

        private ICommand _createFood;
        public ICommand CreateFood
        {
            get
            {
                if (_createFood == null)
                {
                    _createFood = new DelegateCommand(() =>
                    {
                        EditFoodViewModel = new EditFoodViewModel(null, SaveFoodUpdate);
                    });
                }
                return _createFood;
            }
        }

        private ICommand _editSelectedFood;
        public ICommand EditSelectedFood
        {
            get
            {
                if (_editSelectedFood == null)
                {
                    _editSelectedFood = new DelegateCommand(() =>
                    {
                        if (SelectedFood == null) 
                            return;
                        EditFoodViewModel = new EditFoodViewModel(SelectedFood, SaveFoodUpdate);
                    });
                }
                return _editSelectedFood;
            }
        }

        private void SaveFoodUpdate(ConsumableViewModel x)
        {
            EditFoodViewModel = null;
            // new food item
            if (x.Id == null)
            {
                x.Id = Guid.NewGuid().ToString();
                AllFoods.Add(x);
            }
            else
            {
                // Update existing food item
                var food = AllFoods.FirstOrDefault(y => y.Id == x.Id);
                if (food == null)
                {
                    AllFoods.Add(x);
                }
                else
                {
                    food.clone(x);
                }
            }
            SaveFoods();
        }

        private void SaveFoods()
        {
            var json = JsonConvert.SerializeObject(AllFoods);
            File.WriteAllText(filePathFood, json, Encoding.UTF8);
        }

        private void LoadFoods()
        {
            var conten = File.ReadAllText(filePathFood, Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<ConsumableViewModel>>(conten);
            AllFoods.Clear();
            AllFoods.AddRange(list);
        }
        #endregion

        public MainViewModel()
        {
            foodService = new FoodService();
            AllFoods = new ObservableCollection<ConsumableViewModel>();
            LoadFoods();
        }
    }
}
