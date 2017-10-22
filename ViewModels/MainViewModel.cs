using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.Models;
using DietPlanner.Models.SaveModels;
using DietPlanner.Services;
using DietPlanner.ViewModels.Common;
using DietPlanner.ViewModels.Food;
using DietPlanner.ViewModels.Recipies;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ConsumablesViewModel Consumables { get; set; }
        public RecipiesViewModel Recipies { get; set; }
        public PlanViewModel Plan { get; set; }

        public MainViewModel()
        {
            Consumables = new ConsumablesViewModel(this);
            Recipies = new RecipiesViewModel(this);
            Plan = new PlanViewModel(this);
        }
    }
}
