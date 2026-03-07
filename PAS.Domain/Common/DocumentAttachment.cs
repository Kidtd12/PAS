using System;

namespace Domain.Common
{
    public class DocumentAttachment : BaseEntity
    {
        public string FileName { get; private set; }

        public string FilePath { get; private set; }

        public string ContentType { get; private set; }

        public Guid RelatedEntityId { get; private set; }

        public string RelatedEntityName { get; private set; }

        public DocumentAttachment(string fileName, string filePath, string type, Guid entityId, string entityName)
        {
            FileName = fileName;
            FilePath = filePath;
            ContentType = type;
            RelatedEntityId = entityId;
            RelatedEntityName = entityName;
        }
    }
}