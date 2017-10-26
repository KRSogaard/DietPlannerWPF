using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DietPlanner.ViewModels.Shopping;
using RazorEngine;
using RazorEngine.Templating;

namespace DietPlanner.Printing
{
    public class ShoppingListPrinting
    {
        private List<ShoppingListViewModel.ShoppingListItemViewModel> Items;

        public ShoppingListPrinting(List<ShoppingListViewModel.ShoppingListItemViewModel> items)
        {
            Items = items;
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
            string template = File.ReadAllText("Printing/ShoppingListTemplate.cshtml");

            var model = new ShippingListPrintModel()
            {
                Items = Items
            };

            string html;
            if (Engine.Razor.IsTemplateCached("shopping", typeof(ShippingListPrintModel)))
            {
                html = Engine.Razor.RunCompile("shopping", typeof(ShippingListPrintModel), model);
            }
            else
            {
                html = Engine.Razor.RunCompile(template, "shopping", typeof(ShippingListPrintModel), model);
            }
            return html;
        }

        public class ShippingListPrintModel
        {
            public List<ShoppingListViewModel.ShoppingListItemViewModel> Items { get; set; }
        }
    }
}
