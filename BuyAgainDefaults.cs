using System.Collections.Generic;
using Nop.Core;

namespace Nop.Plugin.Widgets.BuyAgain
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public class BuyAgainDefaults
    {
        /// <summary>
        /// The key of the settings to save fixed rate of the shipping method
        /// </summary>
        public const string BuyAgainSettingsKey = "Widgets.BuyAgain.Rate.CategoryId{0}";

        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "Widgets.BuyAgain";

        /// <summary>
        /// Gets the user agent used to request third-party services
        /// </summary>
        public static string UserAgent => $"nopCommerce-{NopVersion.CURRENT_VERSION}";

        /// <summary>
        /// Gets the application name
        /// </summary>
        public static string ApplicationName => "nopCommerce-integration";

        /// <summary>
        /// Gets the partner identifier
        /// </summary>
        public static string PartnerIdentifier => "nopCommerce";

        /// <summary>
        /// Gets the partner affiliation header used for each request to APIs
        /// </summary>
        public static (string Name, string Value) PartnerHeader => ("X-iZettle-Application-Id", "f4954821-e7e4-4fca-854e-e36060b5748d");

       
        /// <summary>
        /// Gets a default period (in seconds) before the request times out
        /// </summary>
        public static int RequestTimeout => 15;

        /// <summary>
        /// Gets a default number of products to import in one request
        /// </summary>
        public static int ImportProductsNumber => 500;

        

        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Widgets.BuyAgain.Configure";

        /// <summary>
        /// Gets the webhook route name
        /// </summary>
        public static string WebhookRouteName => "Plugin.Widgets.BuyAgain.Webhook";

    }
}