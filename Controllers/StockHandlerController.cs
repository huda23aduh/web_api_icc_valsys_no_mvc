using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.Authentication;
using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.ServiceLocation;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts.DataContracts;

using PayMedia.ApplicationServices.Factory;
using PayMedia.ApplicationServices.Logistics.ServiceContracts;
using PayMedia.ApplicationServices.Logistics.ServiceContracts.DataContracts;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class StockHandlerController : ApiController
    {
        static ILogisticsService m_ILogisticsService;

        //[HttpGet]
        //[ActionName("GetStockHandlerByActiveStatus")]
        //[Route("api/stockhandler/GetStockHandlerByActiveStatus/{username_ad}/{password_ad}/{id}")]
        //public StockHandlerCollection GetStockHandlerByActiveStatus(String username_ad, String password_ad, int id)
        //{
        //    #region Here are the parameters you can use with GetStockHandlers()
        //    //Note: Parameters are correct as of MR22.
        //    //Use Intellisense or check the API Reference Library for the latest parameters.
        //    //StockHandler s = new StockHandler();
        //    //s.Active;
        //    //s.AutoReOrder;
        //    //s.BillingReasonChargeSHForMissingStock;
        //    //s.BlockCommission;
        //    //s.ChargeCreditForNonSerialStock;
        //    //s.ChargeReasonForNonSerialStock;
        //    //s.Comment;
        //    //s.Commission;
        //    //s.CommissionAccountId;
        //    //s.CommissionTypeId;
        //    //s.Credit;
        //    //s.CreditAgeFormula;
        //    //s.CreditAmount;
        //    //s.CreditCurrencyId;
        //    //s.CreditReasonForNonSerialStock;
        //    //s.DealerSince;
        //    //s.DealerUntil;
        //    //s.DisplayInStockHandlerlist;
        //    //s.District;
        //    //s.Id;
        //    //s.MissingTimesForChargeStockHandler;
        //    //s.Name;
        //    //s.NextStockTake;
        //    //s.NoBillingReasonChargeSHForMissingStock;
        //    //s.OrderAccountId;
        //    //s.PerformanceCategory;
        //    //s.ShipmentMethodId;
        //    //s.ShippingOrderType;
        //    //s.StockAvailable;
        //    //s.StockTakeFrequency;
        //    //s.Type;
        //    //s.WarehouseId;
        //    #endregion

        //    BaseQueryRequest request = new BaseQueryRequest();
        //    request.PageCriteria = new PageCriteria { Page = 0 };
        //    CriteriaCollection criteria = new CriteriaCollection();

        //    criteria.Add("Active", id);
        //    request.FilterCriteria = criteria;
        //    {
        //        StockHandlerCollection shc = GetSH(request);
        //        if (shc != null && shc.Items.Count > 0)
        //        {
        //            return shc;
        //            //foreach (StockHandler s in shc.Items)
        //            //{
        //            //    Console.WriteLine(string.Format("Found Stockhandler ID: {0} for BusinessIdentifier: {1} -- type : {2} ", s.Id, s.Name, s.Type));
        //            //}
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        //static ILogisticsService LogisticsService
        //{
        //    get
        //    {
        //        if (m_ILogisticsService == null)
        //        {
        //            Authentication_class var_auth = new Authentication_class();
        //            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
        //            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
        //            AsmRepository.AsmOperationTimeout = 360;
        //            m_ILogisticsService = AsmRepository.GetServiceProxyCachedOrDefault<ILogisticsService>(ah);
        //        }
        //        return m_ILogisticsService;
        //    }
        //}

        //static StockHandlerCollection GetSH(BaseQueryRequest baseQueryRequest)
        //{
        //    StockHandlerCollection stockHandlerCollection = null;
        //    ILogisticsService logisticsService = LogisticsService;
        //    stockHandlerCollection = logisticsService.GetStockHandlers(baseQueryRequest);
        //    return stockHandlerCollection;
        //}

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/stockhandler/GetStockHandlerByActiveStatus/{id}")]
        public StockHandlerCollection GetStockHandlerByActiveStatus(String username_ad, String password_ad, int id)
        {

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<ILogisticsService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            request.FilterCriteria.Add(new Criteria("Active", id));

            var servicetype = TSService.GetStockHandlers(request);
            if (servicetype != null)
            {
                return servicetype;
            }
            else
            {
                return null;
            }


        }

    }
}
