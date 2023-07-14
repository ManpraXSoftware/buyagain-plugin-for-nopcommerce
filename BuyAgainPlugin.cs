using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Widgets.BuyAgain.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.BuyAgain
{
    public class BuyAgainPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly WidgetSettings _widgetSettings;

        public BuyAgainPlugin(ISettingService settingService,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            WidgetSettings widgetSettings)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;   
        }

        public bool HideInWidgetList => false;

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(BuyAgainViewComponent);
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
           
            return Task.FromResult<IList<string>>(new List<string>
            {
                PublicWidgetZones.HomepageTop
            });
        }

        public override async Task InstallAsync()
        {
            //settings
            var settings = new BuyAgainSettings
            {
                Enable = false

            };
             await _settingService.SaveSettingAsync(settings);
            //locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.BuyAgain.Fields.BuyAgain"] = "Buy Again",
                ["Plugins.Widgets.BuyAgain.Fields.SeeMore"] = "See more",
                ["Plugins.Widgets.BuyAgain.Fields.Category"] = "Category ",
                ["Plugins.Widgets.BuyAgain.Fields.Category.Hint"] = "Specify Category to display",
                ["Plugins.Widgets.BuyAgain.Fields.Enable"] = "Enabled",
                ["Plugins.Widgets.BuyAgain.Fields.Enable.Hint"] = "Determine whether this feature is enable in home page",


                ["Plugins.Widgets.BuyAgain.Instructions"] = @"
                    <p>
                      BuyAgain Plugin Helps to show Ordered items in HomePage<br>
	                 <br>  
                   </p>",
            });
            await base.InstallAsync();
        }



        public override Task PreparePluginToUninstallAsync()
        {
            throw new NotImplementedException();
        }

        public override async Task UninstallAsync()
        {

            _widgetSettings.ActiveWidgetSystemNames.Remove(BuyAgainDefaults.SystemName);
            //settings
            await _settingService.DeleteSettingAsync<BuyAgainSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.BuyAgain");

            await base.UninstallAsync();
        }
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/BuyAgain/Configure";
        }
    }
}