using System;
using System.Activities;
using System.Linq;
using Microsoft.Xrm.Sdk.Workflow;


namespace msdyncrmWorkflowTools.Class
{
    public class DeleteRecord : CodeActivity
    {
        [RequiredArgument]
        [Input("Delete Using Record URL")]
        [Default("True")]
        public InArgument<bool> DeleteUsingRecordURL { get; set; }

        [Input("Record URL")]
        [ReferenceTarget("")]
        public InArgument<string> DeleteRecordURL { get; set; }

        [Input("Entity Type Name")]
        [ReferenceTarget("")]
        public InArgument<string> EntityTypeName { get; set; }

        [Input("Entity Guid")]
        [ReferenceTarget("")]
        public InArgument<string> EntityGuid { get; set; }


        protected override void Execute(CodeActivityContext executionContext)
        {
            #region "Load CRM Service from context"

            var objCommon = new Common(executionContext);
            objCommon.tracingService.Trace("Load CRM Service from context --- OK");
            #endregion

            #region "Read Parameters"
            var _deleteRecordURL = DeleteRecordURL.Get(executionContext);
            var entityName = string.Empty;
            var objectId = string.Empty;
            if (_deleteRecordURL != null)
            {
                var urlParts = _deleteRecordURL.Split("?".ToArray());
                var urlParams = urlParts[1].Split("&".ToCharArray());
                var objectTypeCode = urlParams[0].Replace("etc=", string.Empty);
                entityName = objCommon.sGetEntityNameFromCode(objectTypeCode, objCommon.service);
                objectId = urlParams[1].Replace("id=", string.Empty);
                objCommon.tracingService.Trace("ObjectTypeCode=" + objectTypeCode + "--ParentId=" + objectId);
            }
            var _deleteUsingRecordURL = DeleteUsingRecordURL.Get(executionContext);
            var _entityTypeName = EntityTypeName.Get(executionContext);
            var _entityGuid = EntityGuid.Get(executionContext);

            #endregion

            #region "Delete Record Execution"

            if (_deleteUsingRecordURL)
            {
                objCommon.tracingService.Trace("Deleting record by URL: {0}", _deleteRecordURL);

                if (_deleteRecordURL == null || _deleteRecordURL == string.Empty )
                {
                    throw new InvalidOperationException("ERROR: Delete Record URL to be deleted missing.");
                }
                objCommon.service.Delete(entityName, new Guid (objectId));
            }
            else
            {
                objCommon.tracingService.Trace("Record type to be deleted: "+ _entityTypeName+" and ID:"+ _entityGuid);
                if (_entityTypeName == null || _entityTypeName == string.Empty || _entityGuid == null || _entityGuid == string.Empty)
                {
                    throw new InvalidOperationException("ERROR: Entity Type name or GUID to be deleted missing.");
                }
                objCommon.tracingService.Trace("Deleting record by Guid: {0}-{1}", _entityTypeName, _entityGuid);
                objCommon.service.Delete(_entityTypeName, new Guid (_entityGuid));
            }


            #endregion

        }
    }
}
