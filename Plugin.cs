using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Net;

// Microsoft Dynamics CRM namespace(s)
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace student4Plugin
{
    public class UpdateResponse : IPlugin
    {
        /// <summary>
        /// A plug-in that creates a follow-up task activity when a new account is created.
        /// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and asynchronous mode.
        /// </remarks>
        /// 
        public void CreateResponse(Guid id, string Response)
        {
            //sending data to the api
            //var data = "{'InquiryId':" + entity.Id + ", 'Response':'my response'}";
            var data = "{InquiryId:" + id.ToString() + ",Response:" + Response + "}";            
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            client.UploadStringAsync(new Uri("http://rest.learncode.academy/api/virginia/inquiries"), "PUT", data);
        }


        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "new_inquiry")
                    return;
                //using (WebClient client = new WebClient())
                //{
                    try
                    {
                    FaultException ex = new FaultException();
                        var response = entity.GetAttributeValue<string>("new_response");
                    tracingService.Trace("Plugin is working");
                    //throw new InvalidPluginExecutionException("Plugin is working", ex);

                    CreateResponse(entity.Id, response);

                    }
                    catch (FaultException<OrganizationServiceFault> ex)
                    {
                        throw new InvalidPluginExecutionException("An error occurred in the FollowupPlugin plug-in.", ex);
                    }

                    catch (Exception ex)
                    {
                        tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                        throw;
                    }

                //}
            }
        }
    }

    public class DeleteQuestion : IPlugin
    {
        /// <summary>
        /// A plug-in that creates a follow-up task activity when a new account is created.
        /// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and asynchronous mode.
        /// </remarks>
        /// 
        public void DestroryQuestion(Guid id)
        {
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            client.UploadStringAsync(new Uri("http://rest.learncode.academy/api/virginia/inquiries"), "DELETE");
        }


        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "new_inquiry")
                    return;
                //using (WebClient client = new WebClient())
                //{
                try
                {
                    FaultException ex = new FaultException();
                    var response = entity.GetAttributeValue<string>("new_response");
                    tracingService.Trace("Plugin is working");
                    //throw new InvalidPluginExecutionException("Plugin is working", ex);

                    CreateResponse(entity.Id, response);

                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in the FollowupPlugin plug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                    throw;
                }

                //}
            }
        }
    }
}
