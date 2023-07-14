using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Widgets.BuyAgain.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.BuyAgain.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class BuyAgainController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly ICategoryService _categoryService;
      

        #endregion

        #region Ctor

        public BuyAgainController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            ICategoryService categoryService
            )
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _categoryService = categoryService;
        }

        #endregion

        #region Methods
              

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            var store = await _storeContext.GetCurrentStoreAsync();

            ////load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<BuyAgainSettings>(storeScope);

            var model = new ConfigurationModel
            {
                CategoryId= Convert.ToInt32(settings.CategoryId),
                ActiveStoreScopeConfiguration = storeScope
            };
            if (storeScope > 0)
            {               
                model.Enable_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.Enable, storeScope);
                model.CategoryId_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.CategoryId, storeScope);
            }

            var categories = await _categoryService.GetAllCategoriesAsync(store?.Id ?? 0);
            if (!categories.Any())
                return Content("No category can be loaded");

            var selectedCategory = await _categoryService.GetCategoryByIdAsync(settings.CategoryId);

            foreach (var sm in categories)
                model.AvailableCategories.Add(new SelectListItem { Text = sm.Name, Value = sm.Id.ToString(), Selected = (selectedCategory != null && sm.Id == selectedCategory.Id) });

            return View("~/Plugins/Widgets.BuyAgain/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<BuyAgainSettings>(storeScope);

            //save settings
            settings.Enable = model.Enable;
            settings.CategoryId = model.CategoryId;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */

            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.Enable, model.Enable_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.CategoryId, model.CategoryId_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }
        
        #endregion
    }
}