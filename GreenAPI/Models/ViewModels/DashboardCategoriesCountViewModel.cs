using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Models.ViewModels
{
    public class DashboardCategoriesCountViewModel
    {
        public string Category { get; set; }
        public int SustainableItemsCount { get; set; }
        public int NonSustainableItemsCount { get; set; }
    }
}
