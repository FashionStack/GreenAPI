using GreenAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int ProductsCount { get; set; }
        public int StockItemsCount { get; set; }
        public int SustainableItemsCount { get; set; }
        public int NonSustainableItemsCount { get; set; }
        public List<DashboardCategoriesCountViewModel> Categories { get; set; }
    }
}
