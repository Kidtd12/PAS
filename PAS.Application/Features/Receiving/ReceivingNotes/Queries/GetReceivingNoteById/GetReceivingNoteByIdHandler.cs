using AutoMapper;
using MediatR;
using PAS.Application.Features.Receiving.ReceivingNotes.Queries.GetReceivingNoteById;

namespace Application.Features.Receiving.ReceivingNotes.Queries;

public class GetReceivingNoteByIdQueryHandler : IRequestHandler<GetReceivingNoteByIdQuery, Result<ReceivingNoteDetailDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReceivingNoteByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<ReceivingNoteDetailDto>> Handle(GetReceivingNoteByIdQuery request, CancellationToken cancellationToken)
    {
        var receivingNote = await _context.ReceivingNotes
            .Include(r => r.Supplier)
            .Include(r => r.ReceivedBy)
            .Include(r => r.InspectionLog)
                .ThenInclude(i => i.Inspector)
            .Include(r => r.Attachments)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);

        if (receivingNote == null)
        {
            throw new NotFoundException(nameof(Domain.Receiving.ReceivingNote), request.Id);
        }

        var noteDto = _mapper.Map<ReceivingNoteDetailDto>(receivingNote);

        // Get items from stock ledger
        var items = await _context.StockLedgers
            .Include(l => l.Item)
            .Where(l => l.ReferenceId == request.Id &&
                       (l.TransactionType == "RECEIVED_PENDING_INSPECTION" ||
                        l.TransactionType == "RECEIVED"))
            .Select(l => new ReceivingNoteItemDto
            {
                Id = Guid.NewGuid(), // Would use actual item ID from a proper ReceivingNoteItem entity
                ItemId = l.ItemId,
                ItemName = l.Item != null ? l.Item.ItemName : string.Empty,
                SKU = l.Item != null ? l.Item.SKU : string.Empty,
                Quantity = l.QuantityChange,
                UnitPrice = 0, // Would need to store price in receiving note items
                UnitOfMeasure = l.Item != null ? l.Item.UnitOfMeasure : string.Empty
            })
            .ToListAsync(cancellationToken);

        noteDto.Items = items;

        // Add inspection info
        if (receivingNote.InspectionLog != null)
        {
            noteDto.Inspection = new InspectionSummaryDto
            {
                Id = receivingNote.InspectionLog.Id,
                InspectionDate = receivingNote.InspectionLog.InspectionDate,
                InspectorName = receivingNote.InspectionLog.Inspector?.Username ?? "Unknown",
                IsPassed = receivingNote.InspectionLog.IsPassed,
                DeviationNotes = receivingNote.InspectionLog.DeviationNotes,
                AcceptedItems = items.Count, // Simplified
                RejectedItems = 0 // Would need to calculate from return requests
            };
        }

        return Result<ReceivingNoteDetailDto>.Success(noteDto);
    }
}