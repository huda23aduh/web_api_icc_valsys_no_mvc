using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SandBoxManager.ServiceContracts;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts.DataContracts;
using web_api_icc_valsys_no_mvc.Models;
using PayMedia.ApplicationServices.Customers.ServiceContracts.DataContracts;
using System.Globalization;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using System.Text;
using Newtonsoft.Json;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class WorkOrderController : ApiController
    {
        StringBuilder er = new StringBuilder();

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/GetWorkOrderByCustomerId/{CustomerId_param}")]
        public HttpResponseMessage GetWorkOrderByCustomerId(String username_ad, String password_ad, int CustomerId_param)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var workforceService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceService>(ah);
            #endregion
            //Instantiate the request object and define filter criteria
            //GetWorkOrdersRequest implements the BaseQueryRequest, so you can filter, sort,
            //and page the returned records.
            //You can use the properties of WorkOrder to filter and sort.
            GetWorkOrdersRequest request = new GetWorkOrdersRequest();
            request.FilterCriteria.Add("CustomerId", CustomerId_param);
            //request.FilterCriteria.Add("WorkOrderStatusId", 1);
            #region Get the customer's work orders and display the results.
            WorkOrderCollection coll = workforceService.GetWorkOrders(request);
            #endregion

            if (coll != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, coll);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/GetWorkOrderByWorkOrderId/{Id_param}")]
        public HttpResponseMessage GetWorkOrderByWorkOrderId(int Id_param, String username_ad, String password_ad)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var workforceService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceService>(ah);
            #endregion
            //Instantiate the request object and define filter criteria
            //GetWorkOrdersRequest implements the BaseQueryRequest, so you can filter, sort,
            //and page the returned records.
            //You can use the properties of WorkOrder to filter and sort.
            GetWorkOrdersRequest request = new GetWorkOrdersRequest();
            request.FilterCriteria.Add("Id", Id_param);
            //request.FilterCriteria.Add("WorkOrderStatusId", 1);
            #region Get the customer's work orders and display the results.
            WorkOrderCollection coll = workforceService.GetWorkOrders(request);
            #endregion

            if (coll != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, coll);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/GetWorkOrderServicesByWorkOrderId/{Id_param}")]
        public HttpResponseMessage GetWorkOrderServicesByWorkOrderId(int Id_param, String username_ad, String password_ad)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var workforceService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceService>(ah);
            #endregion
            //Instantiate the request object and define filter criteria
            //GetWorkOrdersRequest implements the BaseQueryRequest, so you can filter, sort,
            //and page the returned records.
            //You can use the properties of WorkOrder to filter and sort.
            //var request = new GetWorkOrdersRequest();
            //request.FilterCriteria.Add("Id", Id_param);
            //request.FilterCriteria.Add("WorkOrderStatusId", 1);
            #region Get the customer's work orders and display the results.
            WorkOrderServiceCollection coll = workforceService.GetWorkOrderServices(Id_param, 10);
            #endregion

            if (coll != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, coll);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

        }

        [HttpPost]
        [Route("api/workorder/CreateWorkOrder")]
        public WorkOrder CreateWorkOrder([FromBody]WorkOrderParams customer_data)
        {
            int var_geodef_id = 0;
            int var_servprovserv_id = 0;
            int var_starting_timeslot_id = 0;

            String the_DateTime = customer_data.servDateTime + ",531";
            //DateTime myDate = DateTime.ParseExact(the_DateTime, "yyyy-MM-dd hh:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);

            DateTime myDate = DateTime.ParseExact(customer_data.servDateTime, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);


            int dy = myDate.Day;
            int mn = myDate.Month;
            int yy = myDate.Year;

            int hh = myDate.Hour;
            int mm = myDate.Minute;
            int ss = myDate.Second;

            
            // be carefull, if create work order error, dont forget to think about this
            if (hh.ToString() == "09" || hh.ToString() == "9")
                var_starting_timeslot_id = 1;
            else if (hh.ToString() == "12")
                var_starting_timeslot_id = 2;
            else if (hh.ToString() == "13" )
                var_starting_timeslot_id = 3;
            else if (hh.ToString() == "16" )
                var_starting_timeslot_id = 4;
            // be carefull, if create work order error, dont forget to think about this

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(customer_data.username_ad, customer_data.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var workforceService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceService>(ah);
            var sandboxService = AsmRepository.AllServices.GetSandBoxManagerService(ah);

            var customersConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersConfigurationService>(ah);
            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);

            
            //Create the sandbox workspace. A sandbox is required when
            //you create a work order.
            int sandboxId = sandboxService.CreateSandbox();
            #region In this section, we set up the input parameters for CreateWorkOrder.
            //Replace this value with a valid Reason ID for ICC event 55, Create Work Order.
            int reasonKey = 14659;
            //Instantiate a WorkOrder object and set its values.
            WorkOrder workOrder = new WorkOrder();
            //Here we set the work order's sandbox to the sandbox we initialized above.
            //workOrder.SandboxId = 1;
            //Replace this value with the ID of the customer's Agreement.
            workOrder.AgreementId = customer_data.agreementId;
            //Required. Replace this value with the customer's ID.
            workOrder.CustomerId = customer_data.customerId;
            //Required. Replace this value with the ID of the customer's financial account.
            workOrder.FinancialAccountId = customer_data.FinancialAccountId;
            
            //Required. Replace this value with the ID of the service provider.
            workOrder.ServiceProviderId = customer_data.servProvId;
            //Required. Replace this value with the ID of the Work Order Service Type.
            workOrder.ServiceTypeId = customer_data.servTypeId;
            //Replace this value with the date/time when the service should be performed.
            //workOrder.ServiceDateTime = System.DateTime.Now.AddDays(+2);
            workOrder.ServiceDateTime = myDate;

            //Required. Replace this value with the AddressID where the service
            //will be performed.
            workOrder.AddressId = customer_data.servAddressId;

            workOrder.StockHandlerId = 1;

            var_geodef_id = find_geodefinition(customer_data.username_ad, customer_data.password_ad, customer_data.postal_code);
            var_servprovserv_id = getserviceproviderservice_byserviceprovideridandgeoid(customer_data.username_ad, customer_data.password_ad, customer_data.servProvId, customer_data.servTypeId, var_geodef_id);

            workOrder.ServiceProviderServiceId = var_servprovserv_id; //ServiceProviderServiceId

            //Replace the value with a description of the problem.
            workOrder.ProblemDescription = customer_data.ProbDescription;
            //During the finalisation of the sandbox, ICC checks the business rules
            //for all objects.
            //If any object fails this check, the sandbox will fail.
            //If this value is set to true, ICC skips much (but not all) of the
            //business rule validation for this object.
            workOrder.SandboxSkipValidation = false;
            //Required. You must create at least one Work Order Service.
            workOrder.WorkOrderServices = customer_data.the_services;
            //workOrder.WorkOrderServices = new WorkOrderServiceCollection
            //    {
            //        //new WorkOrderService
            //        //{
            //        //    //Required. Replace this value with a valid ID for the work order
            //        //    //service.
            //        //    ServiceId = 3,
            //        //    //Replace this value with the serial number of the device that needs to
            //        //    //be repaired.
            //        //    DeviceSerialNumber = "2010001019",
            //        //    //Replace this value with a valid Reason ID for event 62.
            //        //    //ReasonId = 14659,
            //        //    //Replace this value with how many of the services the customer needs.
            //        //    Quantity = 1
            //        //},

            //        //new WorkOrderService()
            //        //{
            //        //    ServiceId= 70,  //Installation service your CSR choose.
            //        //    //ReasonId = 62,
            //        //    Quantity=1
            //        //},
            //        //new WorkOrderService()
            //        //{
            //        //    ServiceId= 73,  //Installation service your CSR choose.
            //        //    //ReasonId = 62,
            //        //    Quantity=1
            //        //},
            //        new WorkOrderService()
            //        {
            //            ServiceId= 146,      // If CSR choose transport fee.
            //            //SandboxSkipValidation=false,
            //            //ReasonId = 62,
            //            ProductId = 161,
            //            //OverridePrice = 1000000,
            //            Quantity=2
            //        }
            //    };
            #endregion


            #region In this section, we call the method and display the results.
            var wo = workforceService.CreateWorkOrderWithStartingTimeslot(workOrder, new TimeSlotAllocationParameters { StartingTimeSlotId = var_starting_timeslot_id }, reasonKey);
            //WorkOrder wo = workforceService.CreateWorkOrder(workOrder, reasonKey);
            if (wo.WorkOrderServices.Items.Count == 0)
            {
                //Console.WriteLine("Something bad happened. Sandbox rolled back.");
                //Console.ReadLine();
                //The "false" value in this line of code rolls back the sandbox.
                sandboxService.FinalizeSandbox(sandboxId, false);
                return null;
            }
            //The "true" value in this line of code commits the sandbox.
            sandboxService.FinalizeSandbox(sandboxId, true);
            //Console.WriteLine("Congratulations! Created Work Order ID = {0}.", wo.Id);
            //Console.ReadLine();
            #endregion

            //Instantiate the request object and define filter criteria
            //GetWorkOrdersRequest implements the BaseQueryRequest, so you can filter, sort,
            //and page the returned records.
            //You can use the properties of WorkOrder to filter and sort.
            GetWorkOrdersRequest request = new GetWorkOrdersRequest();
            request.FilterCriteria.Add("Id", wo.Id);
            //request.FilterCriteria.Add("WorkOrderStatusId", 1);
            #region Get the customer's work orders and display the results.
            WorkOrderCollection coll = workforceService.GetWorkOrders(request);
            #endregion

            return wo;
            

        }
        public int find_geodefinition(String username_ad, String password_ad, string param_postal_code)
        {
            int var_geodefid = 0;
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var customersConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersConfigurationService>(ah);
            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);

            //Instantiate a request object
            GeoDefinitionCriteria criteria = new GeoDefinitionCriteria();
            //If you know the customer's address ID, you can replace "481" with the addressId
            //and use the returned address to populate the address criteria.
            //Address address = customersService.GetAddress(param_serv_addr_id);
            //If your GeoDefinitions are configured on address details, and if you do not
            //know the customer's addressId, set FindByFullAddressDetails to True and
            //enter the address details.
            //criteria.FindByFullAddressDetails = true;
            //criteria.CountryId = 1;
            //criteria.Province = 1;
            //criteria.BigCity = "LOS ANGELES";
            //criteria.SmallCity = null;
            //criteria.PostalCode = "95BH1";
            //criteria.Street = "VINE ST";
            //If your GeoDefinitions are configured on postal code, and if you do not
            //know the customer's addressId, set FindByPostalCode to True and
            //enter the postal code.
            criteria.FindByPostalCode = true;
            criteria.PostalCode = param_postal_code;
            //Get the GeoDefinition for the address. There should be only one.
            //If there are more than one, then there is an error in the GeoDefinition
            //configuration.
            GeoDefinitionCollection collection = customersConfigurationService.FindGeoDefinitions(criteria);
            //Display the results.
            if (collection != null && collection.Items.Count > 0)
            {
                foreach (var item in collection)
                {
                    var_geodefid = item.Id.Value;
                    //Console.WriteLine("GeoDefId {0} <==> geolocation {1}", item.Id, item.GeoLocations);
                }
                return var_geodefid;
            }
            else
            {
                return 0;
                //Console.WriteLine("I found nothing.");
            }
            //Console.ReadLine();


        }
        public int getserviceproviderservice_byserviceprovideridandgeoid(String username_ad, String password_ad, int param_serv_prov_id, int param_serv_type_id, int param_geodef_id)
        {
            int var_servprovservid = 0;
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var customersConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersConfigurationService>(ah);
            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);
            var aa = AsmRepository.AllServices.GetWorkforceService(ah);

            int[] arr1 = new int[] { param_geodef_id };

            var bb = aa.GetServiceProviderServicesByServiceProviderIdServiceTypeIdAndGeoDefIds(param_serv_prov_id, param_serv_type_id, arr1, 1);

            foreach (var item in bb.Items)
            {
                var_servprovservid = item.Id.Value;
                //Console.WriteLine("serviceproviderserviceid {0} <==> ServiceProviderId {1} <==> servicetytpeid = {2}", item.Id, item.ServiceProviderId, item.ServiceTypeId);
            }

            return var_servprovservid;


        }

        [HttpPost]
        [Route("api/workorder/UpdateWorkOrder")]
        public WorkOrder UpdateWorkOrder([FromBody]UpdateWOParams updated_data)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(updated_data.username_ad, updated_data.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var workforceService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceService>(ah);
            #endregion
            //Instantiate the request object and define filter criteria
            //GetWorkOrdersRequest implements the BaseQueryRequest, so you can filter, sort,
            //and page the returned records.
            //You can use the properties of WorkOrder to filter and sort.
            GetWorkOrdersRequest request = new GetWorkOrdersRequest();
            request.FilterCriteria.Add("Id", updated_data.customerId);
            //request.FilterCriteria.Add("WorkOrderStatusId", 1);
            #region Get the customer's work orders and display the results.
            WorkOrderCollection coll = workforceService.GetWorkOrders(request);

            if (coll != null && coll.Items.Count > 0)
            {
                //Now, instantiate a new WorkOrder and call GetWorkOrder to populate it with the
                // values of the work order you want to change.

                //"1333" is the ID of the work order you want to change.
                WorkOrder workOrder = workforceService.GetWorkOrder(updated_data.workOrderId);
                //Pass in the values you want to change.
                workOrder.ServiceProviderId = 32086;
                workOrder.AddressId = 1215;
                workOrder.ProblemDescription = updated_data.ProbDescription;
                //workOrder.WorkOrderStatusId = 2;

                //"0" is the default reason. ICC throws an error if a default reason is not configured.
                int reasonKey = 0;
                //Call UpdateWorkOrder to update the work order and display the results.
                WorkOrder updatedWorkOrder = workforceService.UpdateWorkOrder(workOrder, reasonKey);
                return updatedWorkOrder;
            }
            else
            {
                return null;
            }
            #endregion
           


        }

        #region OPERATION OF WORKORDER
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/CloseWorkOrderByWorkOrderId/{WOId_param}/{work_desc_param}/{action_taken_param}/{completion_reason_id}")]
        public HttpResponseMessage CloseWorkOrderByWorkOrderId(int WOId_param, String username_ad, String password_ad, string action_taken_param, string work_desc_param, int completion_reason_id)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(WOId_param);  // change 123 to your work order id.
            
            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

            //Assign WorkOder
            //Console.WriteLine("Assgin workorder " + wocs.Id.Value);
            var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);

            // update work order without change status
            //Console.WriteLine("Update work order with out change anything " + wocs.Id.Value);
            wou.ProblemDescription = "It has been changed!!!!!";
            wou = UpdateWorkOrderWithTimeSlot(wou, reason_update_wo, username_ad, password_ad);

            //Change WorK Order to Working status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            wou = UpdateWorkOrderWithTimeSlot(wou, reason_inprogress_wo, username_ad, password_ad);

            // reschedule work order : current work order status must Working
            //Console.WriteLine("Reschedule Work order after wait for rescheduled workorder " + wocs.Id.Value);
            wou = UpdateWorkOrderWithTimeSlot(wou, reason_reschedule_wo, username_ad, password_ad);

            // assign work order after reschedule: current work order status must be Assigned
            //Console.WriteLine("Assgine Work order after reschedule workorder " + wocs.Id.Value);
            wou = UpdateWorkOrderWithTimeSlot(wou, reason_assign_wo, username_ad, password_ad);

            //Change WorKOrder to In Progress status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            wou = UpdateWorkOrderWithTimeSlot(wou, reason_inprogress_wo, username_ad, password_ad);

            // Cancel Work Order, uncomment it if you want to use. When you use it, you can’t complete work order anymore. So comment below complete work order when you use it
            //Console.WriteLine("Cancel workorder " + wou.Id.Value + "");
            //wo.CancelWorkOrder(wou, reason_cancel_wo_RequestedBySub);


            //Complete WorkOrder
            var var_complete_wo = CompleteWorkOrder(wou, reason_complete_wo, username_ad, password_ad, work_desc_param, action_taken_param);
            //Console.WriteLine(DateTime.Now + " : Close workorder " + wou.Id.Value);

            if (wou != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, var_complete_wo);
            }
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", var_complete_wo.Id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            #endregion

        }
        
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/AssignWorkOrderByWorkOrderId/{WOId_param}/{work_desc_param}/{action_taken_param}")]
        public HttpResponseMessage AssignWorkOrderByWorkOrderId(int WOId_param, string work_desc_param, string username_ad, string password_ad, string action_taken_param)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(WOId_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

            //Assign WorkOder
            //Console.WriteLine("Assgin workorder " + wocs.Id.Value);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);
            var wou = UpdateWorkOrderWithOutTimeSlot(wocs, work_desc_param, reason_assign_wo, username_ad, password_ad, action_taken_param);
            // update work order without change status
            //Console.WriteLine("Update work order with out change anything " + wocs.Id.Value);
            wou.ProblemDescription = work_desc_param;
            //wou = UpdateWorkOrderWithTimeSlot(wou, reason_update_wo, username_ad, password_ad);

            //Change WorK Order to Working status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            //wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_inprogress_wo);

            // reschedule work order : current work order status must Working
            //Console.WriteLine("Reschedule Work order after wait for rescheduled workorder " + wocs.Id.Value);
            //wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_reschedule_wo);

            // assign work order after reschedule: current work order status must be Assigned
            //Console.WriteLine("Assgine Work order after reschedule workorder " + wocs.Id.Value);
            //wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_assign_wo);

            //Change WorKOrder to In Progress status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            // wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_inprogress_wo);

            // Cancel Work Order, uncomment it if you want to use. When you use it, you can’t complete work order anymore. So comment below complete work order when you use it
            //Console.WriteLine("Cancel workorder " + wou.Id.Value + "");
            //wo.CancelWorkOrder(wou, reason_cancel_wo_RequestedBySub);


            //Complete WorkOrder
            //var var_complete_wo = CompleteWorkOrder(wou, reason_complete_wo);
            //Console.WriteLine(DateTime.Now + " : Close workorder " + wou.Id.Value);


            if (wou != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, wou);
            }
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", wou.Id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            #endregion


        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/RescheduledToWorkingWorkOrderByWorkOrderId/{WOId_param}/{work_desc_param}/{action_taken_param}")]
        public HttpResponseMessage RescheduledToWorkingWorkOrderByWorkOrderId(int WOId_param, string work_desc_param, string username_ad, string password_ad, string action_taken_param)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(WOId_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

            //Assign WorkOder
            //Console.WriteLine("Assgin workorder " + wocs.Id.Value);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);

            // update work order without change status
            //Console.WriteLine("Update work order with out change anything " + wocs.Id.Value);
            //wou.ProblemDescription = work_desc_param;
            //var wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_update_wo, username_ad, password_ad);

            
            //Change WorKOrder to Working status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            //var wou = UpdateWorkOrderWithOutTimeSlot(wocs, work_desc_param, reason_inprogress_wo, username_ad, password_ad);

            // reschedule work order : current work order status must Working
            //Console.WriteLine("Reschedule Work order after wait for rescheduled workorder " + wocs.Id.Value);
            //wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_reschedule_wo);

            // assign work order after reschedule: current work order status must be Assigned
            //Console.WriteLine("Assgine Work order after reschedule workorder " + wocs.Id.Value);
            var wou = UpdateWorkOrderWithOutTimeSlot(wocs, work_desc_param, reason_assign_wo, username_ad, password_ad, action_taken_param);

            //Change WorKOrder to In Progress status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            wou = UpdateWorkOrderWithOutTimeSlot(wou, work_desc_param, reason_inprogress_wo, username_ad, password_ad, action_taken_param);

            // Cancel Work Order, uncomment it if you want to use. When you use it, you can’t complete work order anymore. So comment below complete work order when you use it
            //Console.WriteLine("Cancel workorder " + wou.Id.Value + "");
            //wou = CancelWorkOrder(wocs, reason_cancel_wo_RequestedBySub, username_ad, password_ad);


            //Complete WorkOrder
            //var var_complete_wo = CompleteWorkOrder(wou, reason_complete_wo);
            //Console.WriteLine(DateTime.Now + " : Close workorder " + wou.Id.Value);

            if (wou != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, wou);
            }
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", wou.Id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            #endregion


        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/AssignToWorkingWorkOrderByWorkOrderId/{WOId_param}/{work_desc_param}/{action_taken_param}")]
        public HttpResponseMessage AssignToWorkingWorkOrderByWorkOrderId(int WOId_param, string work_desc_param, string username_ad, string password_ad, string action_taken_param)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(WOId_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

            //Assign WorkOder
            //Console.WriteLine("Assgin workorder " + wocs.Id.Value);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);

            // update work order without change status
            //Console.WriteLine("Update work order with out change anything " + wocs.Id.Value);
            //wou.ProblemDescription = work_desc_param;
            //var wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_update_wo, username_ad, password_ad);


            //Change WorKOrder to Working status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            var wou = UpdateWorkOrderWithOutTimeSlot(wocs, work_desc_param, reason_inprogress_wo, username_ad, password_ad, action_taken_param);

            // reschedule work order : current work order status must Working
            //Console.WriteLine("Reschedule Work order after wait for rescheduled workorder " + wocs.Id.Value);
            //wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_reschedule_wo);

            // assign work order after reschedule: current work order status must be Assigned
            //Console.WriteLine("Assgine Work order after reschedule workorder " + wocs.Id.Value);
            //var wou = UpdateWorkOrderWithOutTimeSlot(wocs, work_desc_param, reason_assign_wo, username_ad, password_ad);

            //Change WorKOrder to In Progress status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            //wou = UpdateWorkOrderWithOutTimeSlot(wou, work_desc_param, reason_inprogress_wo, username_ad, password_ad);

            // Cancel Work Order, uncomment it if you want to use. When you use it, you can’t complete work order anymore. So comment below complete work order when you use it
            //Console.WriteLine("Cancel workorder " + wou.Id.Value + "");
            //wou = CancelWorkOrder(wocs, reason_cancel_wo_RequestedBySub, username_ad, password_ad);


            //Complete WorkOrder
            //var var_complete_wo = CompleteWorkOrder(wou, reason_complete_wo);
            //Console.WriteLine(DateTime.Now + " : Close workorder " + wou.Id.Value);

            if (wou != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, wou);
            }
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", wou.Id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            #endregion


        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/RescheduleWorkOrderByWorkOrderId/{WOId_param}/{work_desc_param}/{action_taken_param}")]
        public HttpResponseMessage RescheduleWorkOrderByWorkOrderId(int WOId_param, string work_desc_param, string username_ad, string password_ad, string action_taken_param)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(WOId_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

            //Assign WorkOder
            //Console.WriteLine("Assgin workorder " + wocs.Id.Value);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);

            // update work order without change status
            //Console.WriteLine("Update work order with out change anything " + wocs.Id.Value);
            //wou.ProblemDescription = work_desc_param;
            //var wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_update_wo, username_ad, password_ad);

            //Change WorK Order to Working status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            //var wou = UpdateWorkOrderWithOutTimeSlot(wocs, reason_inprogress_wo, username_ad, password_ad);
            
            // reschedule work order : current work order status must Working
            //Console.WriteLine("Reschedule Work order after wait for rescheduled workorder " + wocs.Id.Value);
            var wou = UpdateWorkOrderWithOutTimeSlot(wocs, work_desc_param, reason_reschedule_wo, username_ad, password_ad, action_taken_param);

            // assign work order after reschedule: current work order status must be Assigned
            //Console.WriteLine("Assgine Work order after reschedule workorder " + wocs.Id.Value);
            //wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_assign_wo);

            //Change WorKOrder to In Progress status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            // wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_inprogress_wo);

            // Cancel Work Order, uncomment it if you want to use. When you use it, you can’t complete work order anymore. So comment below complete work order when you use it
            //Console.WriteLine("Cancel workorder " + wou.Id.Value + "");
            //wou = CancelWorkOrder(wocs, reason_cancel_wo_RequestedBySub, username_ad, password_ad);


            //Complete WorkOrder
            //var var_complete_wo = CompleteWorkOrder(wou, reason_complete_wo);
            //Console.WriteLine(DateTime.Now + " : Close workorder " + wou.Id.Value);

            if (wou != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, wou);
            }
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", wou.Id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            #endregion


        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/CompleteWorkOrderByWorkOrderId/{WOId_param}/{work_desc_param}/{action_taken_param}/{completion_reason_id}")]
        public HttpResponseMessage CompleteWorkOrderByWorkOrderId(int WOId_param, string work_desc_param, string username_ad, string password_ad, string action_taken_param, int completion_reason_id)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            #endregion

            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(WOId_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

            //Assign WorkOder
            //Console.WriteLine("Assgin workorder " + wocs.Id.Value);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);
            //var wou = UpdateWorkOrderWithTimeSlot(wocs, reason_assign_wo, username_ad, password_ad);

            // update work order without change status
            //Console.WriteLine("Update work order with out change anything " + wocs.Id.Value);
            //wou.ProblemDescription = work_desc_param;
            //var wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_update_wo, username_ad, password_ad);

            //Change WorK Order to Working status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            //var wou = UpdateWorkOrderWithOutTimeSlot(wocs, reason_inprogress_wo, username_ad, password_ad);

            // reschedule work order : current work order status must Working
            //Console.WriteLine("Reschedule Work order after wait for rescheduled workorder " + wocs.Id.Value);
            //var wou = UpdateWorkOrderWithOutTimeSlot(wocs, reason_reschedule_wo, username_ad, password_ad);
            
            // assign work order after reschedule: current work order status must be Assigned
            //Console.WriteLine("Assgine Work order after reschedule workorder " + wocs.Id.Value);
            //wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_assign_wo);

            //Change WorKOrder to In Progress status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            // wou = UpdateWorkOrderWithOutTimeSlot(wou, reason_inprogress_wo);

            // Cancel Work Order, uncomment it if you want to use. When you use it, you can’t complete work order anymore. So comment below complete work order when you use it
            //Console.WriteLine("Cancel workorder " + wou.Id.Value + "");
            //wou = CancelWorkOrder(wocs, reason_cancel_wo_RequestedBySub, username_ad, password_ad);


            //Complete WorkOrder
            var wou = CompleteWorkOrder(wocs, completion_reason_id,  username_ad, password_ad, work_desc_param, action_taken_param);
            //var temp_work_description = wou.ProblemDescription;
            //wou.ProblemDescription = "#" + temp_work_description;
            //Console.WriteLine(DateTime.Now + " : Close workorder " + wou.Id.Value);

            if (wou != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, wou);
            }
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", wou.Id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        [HttpPost]
        [Route("api/workorder/ForAnggaOnly_ActionWorkOrderByWorkOrderId")]
        public HttpResponseMessage ForAnggaOnly_ActionWorkOrderByWorkOrderId(Angga_Wo the_var)
        {
            
            WorkOrder result_wo_var = null;
            Angga_Wo var_satu = the_var;

            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(the_var.username_ad, the_var.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            #endregion

            string the_json = JsonConvert.SerializeObject(the_var, Formatting.Indented);

            er.Append("RECEIVE JSON PARAMETER - " + DateTime.Now + Environment.NewLine + the_json + Environment.NewLine + Environment.NewLine);
            var_auth.write_log(er);

            #region condition

            if (the_var.wo_action_name_param.ToLower() == "newtoassign")
                result_wo_var = NewToAssignWo_post(var_satu);
            else if (the_var.wo_action_name_param.ToLower() == "assigntoworking")
                result_wo_var = AssignToWorkingWo_post(var_satu);
            else if (the_var.wo_action_name_param.ToLower() == "complete")
                result_wo_var = CompleteWo_post(var_satu);
            else if (the_var.wo_action_name_param.ToLower() == "cancel")
                result_wo_var = CancelWo_post(var_satu);
            else if (the_var.wo_action_name_param.ToLower() == "reschedule")
                result_wo_var = RescheduleWo_post(var_satu);
            else if (the_var.wo_action_name_param.ToLower() == "rescheduletoworking")
                result_wo_var = RescheduleToWorkingWo_post(var_satu);
            else if (the_var.wo_action_name_param.ToLower() == "reassign")
                result_wo_var = ChangeProbDescWo_post(var_satu);

            #endregion condition


            if (result_wo_var != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result_wo_var);
                er.Append("GET RESPONSE - " + DateTime.Now + Environment.NewLine + result_wo_var + Environment.NewLine + Environment.NewLine);
                var_auth.write_log(er);
            }
                
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", the_var.wo_id_param);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);

                er.Append("GET RESPONSE - " + DateTime.Now + Environment.NewLine + message + Environment.NewLine + Environment.NewLine);
                var_auth.write_log(er);
            }

        }

        public WorkOrder CancelWo_post(Angga_Wo the_params)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(the_params.username_ad, the_params.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            #endregion Authenticate and create proxies

            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(the_params.wo_id_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;
            const int reason_cancel_wo_fraud = 99370;
            const int reason_cancel_wo_subscriber_quality = 99368;
            const int reason_cancel_wo_internal_issues = 99369;
            const int reason_cancel_wo_RequestedBySub = 99367;

            const int reason_cancel_wo_technicalissue = 24431;
            //wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            //{
            //    FilterCriteria = new CriteriaCollection() {
            //                new Criteria() {
            //                    Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
            //                },
            //                new Criteria() {
            //                    Key = "Active", Operator=Operator.Equal,Value="true"
            //                },
            //                new Criteria() {
            //                    Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
            //                },
            //            }
            //}).Items[0].Id.Value;

            // Cancel Work Order, uncomment it if you want to use. When you use it, you can’t complete work order anymore. So comment below complete work order when you use it
            var wou = CancelWorkOrder(wocs, reason_cancel_wo_RequestedBySub, the_params.username_ad, the_params.password_ad, the_params.work_desc_param, the_params.action_taken_param);
            var temp_work_description = wou.ProblemDescription;
            wou.ProblemDescription = "#" + temp_work_description;

            er.Append("PROCESS - " + DateTime.Now + Environment.NewLine + wou + Environment.NewLine + Environment.NewLine);
            var_auth.write_log(er);


            if (wou != null)
            {
                return wou;
            }
            else
            {
                return null;
            }
        }

        public WorkOrder CompleteWo_post(Angga_Wo the_params)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(the_params.username_ad, the_params.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            #endregion

            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(the_params.wo_id_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;
            
            
            if(the_params.reason_id == 0)
                the_params.reason_id = reason_complete_wo;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;
            

            //Complete WorkOrder
            var wou = CompleteWorkOrder(wocs, the_params.reason_id, the_params.username_ad, the_params.password_ad, the_params.work_desc_param, the_params.action_taken_param);

            er.Append("PROCESS - " + DateTime.Now + Environment.NewLine + wou + Environment.NewLine + Environment.NewLine);
            var_auth.write_log(er);


            if (wou != null)
            {
                return wou;
            }
            else
            {
                return null;
            }
        }

        public WorkOrder NewToAssignWo_post(Angga_Wo the_params)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(the_params.username_ad, the_params.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(the_params.wo_id_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

            //Assign WorkOder
            var wou = UpdateWorkOrderWithOutTimeSlot(wocs, the_params.work_desc_param, reason_assign_wo, the_params.username_ad, the_params.password_ad, the_params.action_taken_param);

            er.Append("PROCESS - " + DateTime.Now + Environment.NewLine + wou + Environment.NewLine + Environment.NewLine);
            var_auth.write_log(er);

            // update work order without change status
            wou.ProblemDescription = the_params.work_desc_param;
            //wou = UpdateWorkOrderWithTimeSlot(wou, reason_update_wo, username_ad, password_ad);

            

            if (wou != null)
            {
                return wou;
            }
            else
            {
                return null;
            }

            #endregion
        }

        public WorkOrder RescheduleWo_post(Angga_Wo the_params)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(the_params.username_ad, the_params.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            WorkOrder wou = null;

            var custService = AsmRepository.AllServices.GetCustomersService(authHeader);
            var custcService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
            var woService = AsmRepository.AllServices.GetWorkforceService(authHeader);
            var wocService = AsmRepository.AllServices.GetWorkforceConfigurationService(authHeader);

            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(the_params.wo_id_param);  // change 123 to your work order id.
            DateTime the_dateTime = DateTime.ParseExact(the_params.reschedule_date_param, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            //DateTime the_dateTime24 = DateTime.ParseExact(the_params.reschedule_date_param, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
            int reasonid = 20941;

            if (the_params.reason_id == 0)
                the_params.reason_id = reasonid;

            try
            {
                TimeSlotDescription[] timeslots = null;

                var va = custService.GetAddress(wocs.AddressId.Value);

                var geo = custcService.FindGeoDefinitions(new GeoDefinitionCriteria()
                {
                    PostalCode = va.PostalCode
                });

                int spid = wocs.ServiceProviderId.Value;
                int serviceTypeId = wocs.ServiceTypeId.Value;
                int[] geos = new int[1];
                geos[0] = geo.Items[0].Id.Value;

                // Get Service Provider Service Info
                var sps = woService.GetServiceProviderServicesByServiceProviderIdServiceTypeIdAndGeoDefIds(spid, serviceTypeId, geos, 0);

                // Get all avaiable timeslot
                foreach (var sps1 in sps.Items)
                {
                    timeslots = woService.GetTimeSlotsByServiceProviderServiceId(sps1.Id.Value, the_dateTime);
                }

                if (timeslots == null)
                {
                    return null;
                }
                else
                {

                    // Here, i used the first avaiable time slot. For MNC,  please use the timeslot  that the CSR choose. It is better pass TimeSlotDescription as input parameter
                    wou = woService.UpdateWorkOrderWithStartingTimeslot(wocs, new TimeSlotAllocationParameters
                    {
                        StartingTimeSlotId = 1, //hardcode
                        AllocationOverrideType = TimeSlotAllocationOverrideType.Capacity,
                        
                        ServiceDate = the_dateTime, // 
                        ServiceProviderServiceId = wocs.ServiceProviderServiceId
                    }, reasonid);

                    string temp_work_description = wou.ProblemDescription;
                    string temp_action_taken = wou.ActionTaken;
                    

                    wou.ServiceDateTime = the_dateTime;
                    wou.ProblemDescription = temp_work_description + "#" + the_params.work_desc_param;
                    wou.ActionTaken = temp_action_taken + "#" + the_params.action_taken_param;
                    wou.ReasonKey = the_params.reason_id;
                    

                    var wou_1 = woService.UpdateWorkOrder(wou, wou.ReasonKey.Value);

                    er.Append("PROCESS - " + DateTime.Now + Environment.NewLine + wou_1 + Environment.NewLine + Environment.NewLine);
                    var_auth.write_log(er);

                }

            }
            catch (Exception ex)
            {
                return null;
            }
            
            return wou;
        }

        public WorkOrder AssignToWorkingWo_post(Angga_Wo the_params)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(the_params.username_ad, the_params.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(the_params.wo_id_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;
            

            //Change WorKOrder to Working status
            var wou = UpdateWorkOrderWithOutTimeSlot(wocs, the_params.work_desc_param, reason_inprogress_wo, the_params.username_ad, the_params.password_ad, the_params.action_taken_param);

            er.Append("PROCESS - " + DateTime.Now + Environment.NewLine + wou + Environment.NewLine + Environment.NewLine);
            var_auth.write_log(er);

            if (wou != null)
            {
                return wou;
            }
            else
            {
                return null;
            }
            #endregion
        }

        public WorkOrder RescheduleToWorkingWo_post(Angga_Wo the_params)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(the_params.username_ad, the_params.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(the_params.wo_id_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

           

            // assign work order after reschedule: current work order status must be Assigned
            //Console.WriteLine("Assgine Work order after reschedule workorder " + wocs.Id.Value);
            var wou = UpdateWorkOrderWithOutTimeSlot(wocs, the_params.work_desc_param, reason_assign_wo, the_params.username_ad, the_params.password_ad, the_params.action_taken_param);

            //Change WorKOrder to In Progress status
            //Console.WriteLine("change workorder " + wou.Id.Value + " to In Progress.");
            wou = UpdateWorkOrderWithOutTimeSlot(wou, the_params.work_desc_param, reason_inprogress_wo, the_params.username_ad, the_params.password_ad, the_params.action_taken_param);

            er.Append("PROCESS - " + DateTime.Now + Environment.NewLine + wou + Environment.NewLine + Environment.NewLine);
            var_auth.write_log(er);

            if (wou != null)
            {
                return wou;
            }
            else
            {
                return null;
            }
            #endregion
        }

        public WorkOrder ChangeProbDescWo_post(Angga_Wo the_params)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(the_params.username_ad, the_params.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(the_params.wo_id_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;
            const int reason_cancel_wo_fraud = 99370;
            const int reason_cancel_wo_subscriber_quality = 99368;
            const int reason_cancel_wo_internal_issues = 99369;
            const int reason_cancel_wo_RequestedBySub = 99367;
            const int reason_cancel_wo_technicalissue = 24431;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;



            // 
            var wou = UpdateProbDescWorkOrder(wocs, the_params.work_desc_param, reason_update_wo, the_params.username_ad, the_params.password_ad, the_params.action_taken_param);
            var temp_work_description = wou.ProblemDescription;
            wou.ProblemDescription = "#" + temp_work_description;

            er.Append("PROCESS - " + DateTime.Now + Environment.NewLine + wou + Environment.NewLine + Environment.NewLine);
            var_auth.write_log(er);

            if (wou != null)
            {
                return wou;
            }
            else
            {
                return null;
            }
            #endregion
        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/CancelWorkOrderByWorkOrderId/{WOId_param}/{work_desc_param}/{action_taken_param}")]
        public HttpResponseMessage CancelWorkOrderByWorkOrderId(int WOId_param, string work_desc_param, string username_ad, string password_ad, string action_taken_param)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            #endregion Authenticate and create proxies

            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(WOId_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;
            const int reason_cancel_wo_fraud = 99370;
            const int reason_cancel_wo_subscriber_quality = 99368;
            const int reason_cancel_wo_internal_issues = 99369;
            const int reason_cancel_wo_RequestedBySub = 99367;
            const int reason_cancel_wo_technicalissue = 24431;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;

            

            // Cancel Work Order, uncomment it if you want to use. When you use it, you can’t complete work order anymore. So comment below complete work order when you use it
            var wou = CancelWorkOrder(wocs, reason_cancel_wo_RequestedBySub, username_ad, password_ad, work_desc_param, action_taken_param);
            var temp_work_description = wou.ProblemDescription;
            wou.ProblemDescription = "#" + temp_work_description;



            if (wou != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, wou);
            }
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", wou.Id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }


        }

        //ChangeProbDescWorkOrderByWorkOrderId only work for present workorder and future work order
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/workorder/ChangeProbDescWorkOrderByWorkOrderId/{WOId_param}/{work_desc_param}/{action_taken_param}")]
        public HttpResponseMessage ChangeProbDescWorkOrderByWorkOrderId(int WOId_param, string work_desc_param, string username_ad, string password_ad, string action_taken_param)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var wocs = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(WOId_param);  // change 123 to your work order id.

            const int reason_assign_wo = 19545;
            const int reason_inprogress_wo = 99125;
            const int reason_reschedule_wo = 20941;
            const int reason_complete_wo = 23035;
            const int reason_update_wo = 12;
            const int reason_cancel_wo_fraud = 99370;
            const int reason_cancel_wo_subscriber_quality = 99368;
            const int reason_cancel_wo_internal_issues = 99369;
            const int reason_cancel_wo_RequestedBySub = 99367;
            const int reason_cancel_wo_technicalissue = 24431;

            // Edit WorkOrder, only change the service date time, you can change more like technician(AssociatedId) or the problemdescription.

            wocs.AssociateId = AsmRepository.AllServices.GetCustomersService(authHeader).GetAssociatesByRequest(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                            new Criteria() {
                                Key = "CustomerId", Operator=Operator.Equal,Value=wocs.ServiceProviderId.Value.ToString()
                            },
                            new Criteria() {
                                Key = "Active", Operator=Operator.Equal,Value="true"
                            },
                            new Criteria() {
                                Key = "AssociateTypeId", Operator=Operator.Equal,Value="1"
                            },
                        }
            }).Items[0].Id.Value;



            // 
            var wou = UpdateProbDescWorkOrder(wocs, work_desc_param, reason_update_wo, username_ad, password_ad, action_taken_param);
            var temp_work_description = wou.ProblemDescription;
            wou.ProblemDescription = "#" + temp_work_description;

            if (wou != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, wou);
            }
            else
            {
                var message = string.Format("An Error Has Occured With WorkOrder ID = ", wou.Id);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            #endregion


        }
        
        #endregion OPERATION OF WORKORDER


        public WorkOrder CompleteWorkOrder(WorkOrder wo_param, int reason_param, String username_ad, String password_ad, string work_desc_param, string action_taken)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var woService = AsmRepository.AllServices.GetWorkforceService(authHeader);
            var wo = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(wo_param.Id.Value);
           WorkOrder ww = null;
            try
            {
                string temp_work_description = wo_param.ProblemDescription;
                string temp_action_taken = wo_param.ActionTaken;
                wo.ProblemDescription = temp_work_description + "#" + work_desc_param;
                wo.ActionTaken = temp_action_taken + "#" + action_taken;
                wo.CompletedDateTime = DateTime.Now;
                wo.ReasonKey = reason_param;

                ww = woService.CompleteWorkOrder(wo, reason_param);
                //return ww;
                
            }
            catch (Exception ex)
            {
                //msg = "Exceptions : " + ex.Message + " Stack : " + ex.StackTrace;
                //Console.WriteLine(msg);
            }
            //
            //Console.WriteLine(msg);
            return ww;
        }
        public WorkOrder UpdateWorkOrderWithTimeSlot(WorkOrder wo, int reasonid, String username_ad, String password_ad)
        {

            WorkOrder wou = null;
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var custService = AsmRepository.AllServices.GetCustomersService(authHeader);
            var custcService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
            var woService = AsmRepository.AllServices.GetWorkforceService(authHeader);
            var wocService = AsmRepository.AllServices.GetWorkforceConfigurationService(authHeader);
            try
            {
                TimeSlotDescription[] timeslots = null;

                var va = custService.GetAddress(wo.AddressId.Value);

                var geo = custcService.FindGeoDefinitions(new GeoDefinitionCriteria()
                {
                    PostalCode = va.PostalCode
                });

                int spid = wo.ServiceProviderId.Value;
                int serviceTypeId = wo.ServiceTypeId.Value;
                int[] geos = new int[1];
                geos[0] = geo.Items[0].Id.Value;

                // Get Service Provider Service Info
                var sps = woService.GetServiceProviderServicesByServiceProviderIdServiceTypeIdAndGeoDefIds(
                    spid, serviceTypeId, geos, 0
                );

                // Get all avaiable timeslot
                foreach (var sps1 in sps.Items)
                {
                    timeslots = woService.GetTimeSlotsByServiceProviderServiceId(sps1.Id.Value, DateTime.Now);
                }

                if (timeslots == null)
                {
                    Console.WriteLine("No avaiable time slot , please check the setting! work order id : " + wo.Id.Value);
                    return null;
                }
                else
                {
                    // Here, i used the first avaiable time slot. For MNC,  please use the timeslot  that the CSR choose. It is better pass TimeSlotDescription as input parameter
                    wou = woService.UpdateWorkOrderWithStartingTimeslot(wo, new TimeSlotAllocationParameters
                    {
                        StartingTimeSlotId = timeslots[0].TimeSlotId.Value,
                        AllocationOverrideType = TimeSlotAllocationOverrideType.Capacity,
                        ServiceDate = DateTime.Now.AddDays(2), // 
                        ServiceProviderServiceId = timeslots[0].ServiceProviderServiceId.Value
                    }, reasonid);
                }

            }
            catch (Exception ex)
            {
                //msg = "Exceptions : " + ex.Message + " Stack : " + ex.StackTrace;
                //Console.WriteLine(msg);
            }
            //msg = "Work Order " + wo.Id + " Updated by using reason id : " + reasonid;
            //Console.WriteLine(msg);
            return wou;
        }
        public WorkOrder UpdateWorkOrderWithOutTimeSlot(WorkOrder wo, string work_desc_param, int reason_id, String username_ad, String password_ad, string action_taken)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var woService = AsmRepository.AllServices.GetWorkforceService(authHeader);
            try
            {
                //wo.ProblemDescription = wo.ProblemDescription + " updated asas by reason" + reason_id;
                string temp_work_description = wo.ProblemDescription;
                string temp_action_taken = wo.ActionTaken;
                wo.ProblemDescription = temp_work_description + "#" + work_desc_param;
                wo.ActionTaken = temp_action_taken + "#" + action_taken;
                //wo.ServiceDateTime = wo.ServiceDateTime.Value.AddDays(2);
                //DateTime.Now.AddDays(2);

                WorkOrder ww = woService.UpdateWorkOrder(wo, reason_id);
                return ww;
            }
            catch (Exception ex)
            {
                // msg = "Exceptions : " + ex.Message + " Stack : " + ex.StackTrace;
                //Console.WriteLine(msg);
                return null;
            }

        }
        public WorkOrder UpdateProbDescWorkOrder(WorkOrder wo, string prob_desc, int reason_id, String username_ad, String password_ad, string action_taken)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            IWorkforceService woService = AsmRepository.AllServices.GetWorkforceService(authHeader);
            //WorkOrder wou = AsmRepository.AllServices.GetWorkforceService(authHeader).GetWorkOrder(wo.Id.Value);
            try
            {

                string temp_work_description = wo.ProblemDescription;
                string temp_action_taken = wo.ActionTaken;
                wo.ProblemDescription = temp_work_description + "#" + prob_desc;
                wo.ActionTaken = temp_action_taken + "#" + action_taken;
                //wo.ServiceDateTime = DateTime.Now.AddDays(2);
                WorkOrder ww = woService.UpdateWorkOrder(wo, reason_id);
                return wo;
            }
            catch (Exception ex)
            {
                // msg = "Exceptions : " + ex.Message + " Stack : " + ex.StackTrace;
                //Console.WriteLine(msg);
                return null;
            }

        }
        public WorkOrder CancelWorkOrder(WorkOrder wo, int reason_id, String username_ad, String password_ad, string work_desc_param, string action_taken)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            IWorkforceService woService = AsmRepository.AllServices.GetWorkforceService(authHeader);

            //WorkOrder wou = null;
            try
            {
                string temp_work_description = wo.ProblemDescription;
                string temp_action_taken = wo.ActionTaken;
                wo.ProblemDescription = temp_work_description + "#" + work_desc_param;
                wo.ActionTaken = temp_action_taken + "#" + action_taken;
                //wo.CompletedDateTime = DateTime.Now;
                //wo.CancelledDateTime = DateTime.Now;

                WorkOrder ww = woService.CancelWorkOrder(wo, reason_id);
                return ww;
            }
            catch (Exception ex)
            {
                //msg = "Exceptions : " + ex.Message + " Stack : " + ex.StackTrace;
                //Console.WriteLine(msg);
                return null;
            }

        }


    }
}
