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

        public DocumentAttachment(string fileName, string filePath, string contentType, Guid relatedEntityId, string relatedEntityName)
        {
            FileName = fileName;
            FilePath = filePath;
            ContentType = contentType;
            RelatedEntityId = relatedEntityId;
            RelatedEntityName = relatedEntityName;
        }
    }
}