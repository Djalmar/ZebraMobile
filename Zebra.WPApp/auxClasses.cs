using System.Collections.Generic;
using ZebrasLib.Classes;

namespace Zebra.WPApp.Pages.Places
{
    public class bindingCategory
    {
        public Category category { get; set; }
        public List<Place> lstPlaces { get; set; }
    }

    public class staticClasses
    { 
        public static Place selectedPlace{ get; set; }
    }
}