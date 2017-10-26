using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DietPlanner.ViewModels.Plan;
using DietPlanner.ViewModels.Shopping;
using RazorEngine;
using RazorEngine.Templating;

namespace DietPlanner.Printing
{
    public class PlanDayPrinting
    {
        private PlanDayViewModel Plan;

        public PlanDayPrinting(PlanDayViewModel plan)
        {
            Plan = plan;
        }

        public void Print()
        {
            WebBrowser webBrowser = new WebBrowser();

            webBrowser.DocumentText = getHtmlDocument();

            webBrowser.DocumentCompleted +=
                new WebBrowserDocumentCompletedEventHandler(PrintDocument);
        }

        private void PrintDocument(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            // Print the document now that it is fully loaded.
            ((WebBrowser)sender).Print();

            // Dispose the WebBrowser now that the task is complete. 
            ((WebBrowser)sender).Dispose();
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
