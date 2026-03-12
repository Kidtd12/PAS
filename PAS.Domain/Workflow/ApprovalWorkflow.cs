using Domain.Common;

namespace Domain.Workflow
{
    public class ApprovalWorkflow : BaseEntity
    {
        public string WorkflowName { get; private set; }

        public string Description { get; private set; }

        private ApprovalWorkflow()
        {
        }

        public ApprovalWorkflow(string name, string description)
        {
            WorkflowName = name;
            Description = description;
        }
    }
}
