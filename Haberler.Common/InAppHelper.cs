using Library10.Common.Entities;
using Library10.Common.Enums;
using Library10.Net;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace Haberler.Common
{
    public class InAppHelper
    {
        private static RESTService _service = null;

        public static RESTService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new RESTService();
                    _service.SetBasicAuthentication(Settings.General.AppNameCapitalized, Settings.General.AppMarketId);
                }

                return _service;
            }
        }

        private static bool? _isTrial;

        public static bool IsTrial
        {
            get
            {
                if (!_isTrial.HasValue)
                {
#if DEBUG
                    if (CurrentAppSimulator.LicenseInformation.ProductLicenses[Settings.Product.ProductKey].IsActive)
#else
                    if (CurrentApp.LicenseInformation.ProductLicenses[Settings.Product.ProductKey].IsActive)
#endif
                        _isTrial = false;
                    else
                        _isTrial = true;

#if DEBUG
                    _isTrial = false;
#endif
                }

                return _isTrial.Value;
            }
            set { _isTrial = value; }
        }

        async static public Task<BuyStatus> BuyApplication()
        {
            try
            {
                if (IsTrial)
                {
#if DEBUG
                    var result = await CurrentAppSimulator.RequestProductPurchaseAsync(Settings.Product.ProductKey);
#else
                    var result = await CurrentApp.RequestProductPurchaseAsync(Settings.Product.ProductKey);
#endif
                    _isTrial = null;

                    switch (result.Status)
                    {
                        case ProductPurchaseStatus.Succeeded:
                            await Service.Execute(ServiceCommands.PurchaseApplication(
                                new ApplicationPurchase()
                                {
                                    Id = Settings.General.AppId
                                }
                                ));

                            return BuyStatus.Bought;

                        case ProductPurchaseStatus.AlreadyPurchased:
                            return BuyStatus.AlreadyBought;

                        case ProductPurchaseStatus.NotFulfilled:
                            return BuyStatus.NotFulfilled;

                        case ProductPurchaseStatus.NotPurchased:
                            return BuyStatus.NotPurchased;
                    }
                }

                return BuyStatus.AlreadyBought;
            }
            catch
            {
                return BuyStatus.Error;
            }
        }
    }
}