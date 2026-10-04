using System;
using System.Activities;
using System.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;


namespace msdyncrmWorkflowTools
{
    public class ApplyRoutingRule : CodeActivity
    {
        #region "Parameter Definition"
        [RequiredArgument]
        [Input("Incident Record URL")]
        [ReferenceTarget("")]
        public InArgument<string> IncidentRecordURL { get; set; }
        #endregion
        protected override void Execute(CodeActivityContext executionContext)
        {

            #region "Load CRM Service from context"

            var objCommon = new Common(executionContext);
            objCommon.tracingService.Trace("Load CRM Service from context --- OK");
            #endregion

            #region "Read Parameters"
            var _IncidentRecordURL= IncidentRecordURL.Get(executionContext);
            if (_IncidentRecordURL == null || _IncidentRecordURL == string.Empty)
            {
                return;
            }
            var urlParts = _IncidentRecordURL.Split("?".ToArray());
            var urlParams = urlParts[1].Split("&".ToCharArray());
            var ParentObjectTypeCode = urlParams[0].Replace("etc=", string.Empty);
            var ParentId = urlParams[1].Replace("id=", string.Empty);
            objCommon.tracingService.Trace("ParentObjectTypeCode=" + ParentObjectTypeCode + "--ParentId=" + ParentId);
            #endregion


            #region "ApplyRoutingRuleRequest Execution"
            var EntityName = objCommon.sGetEntityNameFromCode(ParentObjectTypeCode, objCommon.service);
            var routeRequest = new ApplyRoutingRuleRequest();
            routeRequest.Target = new EntityReference(EntityName, new Guid(ParentId));
            var routeResponse = (ApplyRoutingRuleResponse)objCommon.service.Execute(routeRequest);
            
            #endregion

        }
    }
}
