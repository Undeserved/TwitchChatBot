using MediatR;
using QuoteDatabaseChatBot.Application.Common.Interfaces;
using QuoteDatabaseChatBot.Domain.Entities;

namespace QuoteDatabaseChatBot.Application.Quotes.Commands {
    public class UpsertQuoteCommand : IRequest {
        public int QuoteId { get; set; }
        public string? Quote { get; set; }
        public string? Game { get; set; }
        public DateTime QuoteDate { get; set; }
    }

    public class UpsertQuoteHandler : IRequestHandler<UpsertQuoteCommand> {
        private readonly IQuoteCollectionDbContext _context;

        public UpsertQuoteHandler(IQuoteCollectionDbContext context) {
            _context = context;
        }

        public async Task<Unit> Handle(UpsertQuoteCommand request, CancellationToken cancellationToken) {
            Quote? quote = _context.Quotes
                .Where(x => x.Id == request.QuoteId)
                .FirstOrDefault();

            if (quote == null) {
                quote = new Quote { Id = request.QuoteId };
                _context.Quotes.Add(quote);
            }

            quote.Content = request.Quote ?? String.Empty;
            quote.Game = request.Game ?? String.Empty;
            quote.Date = request.QuoteDate;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
