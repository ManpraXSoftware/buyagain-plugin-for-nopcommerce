using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.BuyAgain.Models
{
    public record ConfigurationModel : BaseNopEntityModel
    {
        public ConfigurationModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }
        public int ActiveStoreScopeConfiguration { get; set; }
       
        public int CategryId { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BuyAgain.Fields.Categry")]
        public SelectList? CategryValues { get; set; }
        public bool CategryId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BuyAgain.Fields.Category")]
        public int CategoryId { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BuyAgain.Fields.Category")]
        public string? CategoryName { get; set; }
        public bool CategoryId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BuyAgain.Fields.Enable")]
        public bool Enable { get; set; }
        public bool Enable_OverrideForStore { get; set; }



        public IList<SelectListItem> AvailableCategories { get; set; }

    }
}