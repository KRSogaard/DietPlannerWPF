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
        

        public MainViewModel()
        {
            foodService = new FoodService();
            AllFoods = new ObservableCollection<ConsumableViewModel>();
        }
    }
}
