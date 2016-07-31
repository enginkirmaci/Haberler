using Library10.Common.Entities;
using Library10.Net.Entities;
using Library10.Net.Enums;

namespace Haberler.Common
{
    public class ServiceCommands
    {
        public static RESTCommand<ApplicationPurchase, ApplicationPurchase> PurchaseApplication(ApplicationPurchase purchase)
        {
            var command = new RESTCommand<ApplicationPurchase, ApplicationPurchase>()
            {
                ServiceUrl = Settings.General.ServiceUrl,
                Query = "api/PurchaseApplication",
                Type = RESTCommandType.POST,
                Body = purchase
            };

            return command;
        }
    }
}