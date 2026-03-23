using Application.Common.Security;
using Application.Features.Common.DocumentAttachments.Dtos;
using MediatR;
using DocFileDto = Application.Features.Common.DocumentAttachments.Dtos.FileDto;

namespace Application.Features.Common.DocumentAttachments.Commands.DownloadDocument;

[Authorize(Permissions = Permissions.Documents.Download)]
public record DownloadDocumentCommand(Guid Id) : IRequest<Result<DocFileDto>>;
