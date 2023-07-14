using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Widgets.BuyAgain.Models;
using Nop.Services.Catalog;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Infrastructure.Cache;
using Nop.Web.Models.Media;
using Nop.Web.Models.Order;

namespace Nop.Plugin.Widgets.BuyAgain.Components
{
    public class BuyAgainViewComponent : NopViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IOrderService _orderService;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly MediaSettings _mediaSettings;
        private readonly BuyAgainSettings _settings;
        private readonly ICategoryService _categoryService;
        private readonly IWidgetPluginManager _widgetPluginManager;
        private readonly BuyAgainSettings _buyAgainSettings;

        public BuyAgainViewComponent(IWorkContext workContext,
            IStoreContext storeContext,
            IOrderService orderService,
            IOrderModelFactory orderModelFactory,
            MediaSettings mediaSettings,
            BuyAgainSettings settings,
            ICategoryService categoryService,
            IWidgetPluginManager widgetPluginManager,
            BuyAgainSettings buyAgainSettings)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _orderService = orderService;
            _orderModelFactory = orderModelFactory;
            _mediaSettings = mediaSettings;
            _settings = settings;
            _categoryService = categoryService;
            _widgetPluginManager = widgetPluginManager;
            _buyAgainSettings = buyAgainSettings;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            //customer-details
            var customer = await _workContext.GetCurrentCustomerAsync();
            var store = await _storeContext.GetCurrentStoreAsync();
            var orders = await _orderService.SearchOrdersAsync(storeId: store.Id,
                customerId: customer.Id); //5,7,8 orders table
            var orderItemModel = new List<OrderDetailsModel.OrderItemModel>();

            //ensure that what3words widget is active and enabled
            if (!await _widgetPluginManager.IsPluginActiveAsync(BuyAgainDefaults.SystemName, customer))
                return Content(string.Empty);

            if (!_buyAgainSettings.Enable)
                return Content(string.Empty);

            if (orders == null || orders.Count == 0)
            {
                return View("~/Plugins/Widgets.BuyAgain/Views/PublicInfo2.cshtml", orderItemModel);
            }
            var categoryId = _settings.CategoryId;

            //get order details
            foreach (var order in orders)
            {
                _mediaSettings.OrderThumbPictureSize = 415;
                var model = await _orderModelFactory.PrepareOrderDetailsModelAsync(order);
                foreach (var item in model.Items)
                {
                    //select products by categoryid
                    var productCategory = (_categoryService.GetProductCategoriesByProductIdAsync(item.ProductId)).Result.FirstOrDefault();
                    var sameitems = orderItemModel.Where(o => o.ProductId == productCategory.ProductId);
                    if (_settings.CategoryId == productCategory.CategoryId)
                    {                        
                            orderItemModel.Add(item);
                    }                    
                }                
            }
            //check model values is less than 4 values
            if(orderItemModel.Count < 4 || orderItemModel.Count == 0)
            {
                orderItemModel.Clear();
                return View("~/Plugins/Widgets.BuyAgain/Views/PublicInfo2.cshtml", orderItemModel);
            }
            if (orderItemModel.Count > 4)
            {
                orderItemModel.Take(4);
            }
            return View("~/Plugins/Widgets.BuyAgain/Views/PublicInfo2.cshtml", orderItemModel);
        }              

    }
}