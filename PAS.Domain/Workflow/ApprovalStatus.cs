using Domain.Common;

namespace Domain.Workflow
{
    public class ApprovalStatus : BaseEntity
    {
        public string StatusName { get; private set; }

        public ApprovalStatus(string name)
        {
            StatusName = name;
        }
    }
}
