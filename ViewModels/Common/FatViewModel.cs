using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class FatViewModel : BindableBase
    {
        private double _total;
        public double Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        private double _saturated;
        public double Saturated
        {
            get { return _saturated; }
            set { SetProperty(ref _saturated, value); }
        }

        private double _polyunsaturated;
        public double Polyunsaturated
        {
            get { return _polyunsaturated; }
            set { SetProperty(ref _polyunsaturated, value); }
        }

        private double _monounsaturated;
        public double Monounsaturated
        {
            get { return _monounsaturated; }
            set { SetProperty(ref _monounsaturated, value); }
        }

        private double _trans;
        public double Trans
        {
            get { return _trans; }
            set { SetProperty(ref _trans, value); }
        }
    }
}
