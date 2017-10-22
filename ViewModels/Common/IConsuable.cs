using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.ViewModels.Common
{
    public interface IConsumableViewModel
    {
        string Name { get; }

        string Unit { get; }

        double Calories { get; }

        double FatTotal { get; }

        double Cholesterol { get; }

        double Sodium { get; }

        double CarbohydrateTotal { get; }

        double Protein { get; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
