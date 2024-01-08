using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuoteDatabaseChatBot.Application.Common.Dtos;
using QuoteDatabaseChatBot.Application.Common.Extensions;
using QuoteDatabaseChatBot.Application.Common.Interfaces;
using QuoteDatabaseChatBot.Domain.Entities;

namespace QuoteDatabaseChatBot.Application.Quotes.Queries {
    public class PullQuoteQuery : IRequest<QuoteDto> {
        public int QuoteId { get; set; }
    }

    public class PullQuoteHandler : IRequestHandler<PullQuoteQuery, QuoteDto> {
        private readonly IQuoteCollectionDbContext _context;
        private readonly Random _rngGen;

        public PullQuoteHandler(IQuoteCollectionDbContext context) {
            _context = context;
            _rngGen = new Random();
        }

        public async Task<QuoteDto> Handle(PullQuoteQuery request, CancellationToken cancellationToken) {
            if(request.QuoteId == 0) {
                int quoteCount = _context.Quotes.Count();
                request.QuoteId = _rngGen.RollDice(quoteCount);
            }

            return await _context.Quotes
                .Where(x => x.Id == request.QuoteId)
                .Select(x => new QuoteDto {
                    QuoteId = x.Id,
                    Date = x.Date,
                    Game = x.Game,
                    QuoteContent = x.Content
                })
                .FirstOrDefaultAsync(cancellationToken) ?? new QuoteDto { QuoteId = request.QuoteId };
        }
    }
}
