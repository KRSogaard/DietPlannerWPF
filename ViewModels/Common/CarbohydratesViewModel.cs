using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Common
{
    public class CarbohydratesViewModel : BindableBase
    {
        private double _total;
        public double Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        private double _dietaryFiber;
        public double DietaryFiber
        {
            get { return _dietaryFiber; }
            set { SetProperty(ref _dietaryFiber, value); }
        }

        private double _sugar;
        public double Sugar
        {
            get { return _sugar; }
            set { SetProperty(ref _sugar, value); }
        }
    }
}
