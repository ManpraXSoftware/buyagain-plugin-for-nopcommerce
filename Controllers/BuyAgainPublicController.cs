using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Media;
using Nop.Plugin.Widgets.BuyAgain.Models;
using Nop.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Web.Factories;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.Order;

namespace Nop.Plugin.Widgets.BuyAgain.Controllers
{
    
    public class BuyAgainPublicController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;      
        private readonly IWorkContext _workContext;
        private readonly BuyAgainSettings _settings;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrderService _orderService;


        #endregion

        #region Ctor

        public BuyAgainPublicController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            IWorkContext workContext,
            BuyAgainSettings settings,
            IOrderModelFactory orderModelFactory,
            IOrderService orderService
            )
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _workContext = workContext;
            _settings = settings;
            _orderModelFactory= orderModelFactory;
            _orderService= orderService;
        }

        #endregion

        #region Methods

        public virtual async Task<IActionResult> Seemore()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var store = await _storeContext.GetCurrentStoreAsync();
            var orders = await _orderService.SearchOrdersAsync(storeId: store.Id,
                customerId: customer.Id); //5,7,8 orders table
            var ordermodel = new List<OrderDetailsModel>();
            //get order details
            foreach (var order in orders)
            {
                var model = await _orderModelFactory.PrepareOrderDetailsModelAsync(order);
                ordermodel.Add(model);

            }


            return View("~/Plugins/Widgets.BuyAgain/Views/PublicInfo.cshtml");//, ordermodel);
        }

        #endregion
    }
}