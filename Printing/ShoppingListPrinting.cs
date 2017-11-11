using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DietPlanner.ViewModels;
using DietPlanner.ViewModels.Shopping;
using RazorEngine;
using RazorEngine.Templating;

namespace DietPlanner.Printing
{
    public class ShoppingListPrinting
    {
        private string folder;
        private List<ShoppingListViewModel.ShoppingListItemViewModel> Items;

        public ShoppingListPrinting(MainViewModel mainViewModel, List<ShoppingListViewModel.ShoppingListItemViewModel> items)
        {
            Items = items;
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
