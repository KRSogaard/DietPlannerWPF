using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DietPlanner.ViewModels;
using DietPlanner.ViewModels.Plan;
using DietPlanner.ViewModels.Shopping;
using RazorEngine;
using RazorEngine.Templating;

namespace DietPlanner.Printing
{
    public class PlanDayPrinting
    {
        private string folder; 

        private PlanDayViewModel Plan;

        public PlanDayPrinting(MainViewModel mainViewModel, PlanDayViewModel plan)
        {
            Plan = plan;
            folder = mainViewModel.Settings.TempPath;
        }

        public void Print()
        {
            string path = Path.GetFullPath(folder + Guid.NewGuid() + ".html");
            File.WriteAllText(path, getHtmlDocument());
            System.Diagnostics.Process.Start(path);
        }

        private string getHtmlDocument()
        {
            string template = File.ReadAllText("Printing/PlanDayPrinting.cshtml");
            var model = new ShippingListPrintModel()
            {
                Plan = Plan
            };

            string html;
            if (Engine.Razor.IsTemplateCached("planday", typeof(ShippingListPrintModel)))
            {
                html = Engine.Razor.RunCompile("planday", typeof(ShippingListPrintModel), model);
            }
            else
            {
                html = Engine.Razor.RunCompile(template, "planday", typeof(ShippingListPrintModel), model);
            }
            return html;
        }

        public class ShippingListPrintModel
        {
            public PlanDayViewModel Plan { get; set; }
        }
    }
}
