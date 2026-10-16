using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace msdyncrmWorkflowTools
{
    /// <summary>
    /// Clones a record and its child records: the copy of each child belongs to the copy of the record, the way
    /// Clone Record followed by Clone Children would do it in one step.
    /// </summary>
    [ActivityName("Clone Record With Children")]
    public class CloneRecordWithChildren : WorkflowActivityBase
    {
        [RequiredArgument]
        [Input("Source Record URL")]
        [ReferenceTarget("")]
        public InArgument<string> SourceRecordUrl { get; set; }

        [RequiredArgument]
        [Input("Relationship Names")]
        public InArgument<string> RelationshipNames { get; set; }

        [Input("Prefix")]
        [Default("")]
        public InArgument<string> Prefix { get; set; }

        [Input("Fields to Ignore")]
        [Default("")]
        public InArgument<string> FieldstoIgnore { get; set; }

        [Input("Child Prefix")]
        [Default("")]
        public InArgument<string> ChildPrefix { get; set; }

        [Input("Child Fields to Ignore")]
        [Default("")]
        public InArgument<string> ChildFieldstoIgnore { get; set; }

        [Input("Copy Child Status")]
        [Default("False")]
        public InArgument<bool> CopyChildStatus { get; set; }

        [Input("Only Active Children")]
        [Default("False")]
        public InArgument<bool> OnlyActiveChildren { get; set; }

        [Output("Cloned Guid")]
        public OutArgument<string> ClonedGuid { get; set; }

        [Output("Children Cloned")]
        public OutArgument<int> ChildrenCloned { get; set; }

        protected override void ExecuteActivity(CodeActivityContext executionContext, Common common)
        {
            var createdGuid = common.CloneRecordWithChildren(SourceRecordUrl.Get(executionContext), RelationshipNames.Get(executionContext),
                FieldstoIgnore.Get(executionContext), Prefix.Get(executionContext), ChildFieldstoIgnore.Get(executionContext), ChildPrefix.Get(executionContext),
                CopyChildStatus.Get(executionContext), OnlyActiveChildren.Get(executionContext), out var childrenCloned);

            ClonedGuid.Set(executionContext, createdGuid.ToString());
            ChildrenCloned.Set(executionContext, childrenCloned);
        }
    }
}
