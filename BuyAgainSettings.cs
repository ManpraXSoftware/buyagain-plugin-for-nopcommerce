//using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Configuration;
//using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.BuyAgain
{
    public class BuyAgainSettings : ISettings
    {
        /// <summary>
        /// Gets or sets category list
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feature is enabled in category or not
        /// </summary>
        public bool Enable { get; set; }

        
    }
    
}
