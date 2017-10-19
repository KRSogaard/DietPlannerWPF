using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.Models;
using DietPlanner.Services;
using DietPlanner.ViewModels.Common;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private FoodService foodService;

        public ObservableCollection<ConsumableViewModel> AllFoods { get; set; }

        private ConsumableViewModel _editFoodViewModel;
        public ConsumableViewModel EditFoodViewModel
        {
            get { return _editFoodViewModel; }
            set
            {
                SetProperty(ref _editFoodViewModel, value);
                ShowEditFood = value != null;
            }
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
                        EditFoodViewModel = new ConsumableViewModel();
                    });
                }
                return _createFood;
            }
        }


        public MainViewModel()
        {
            foodService = new FoodService();
            AllFoods = new ObservableCollection<ConsumableViewModel>();
        }
    }
}
