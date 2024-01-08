using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuoteDatabaseChatBot.Application.Common.Dtos;
using QuoteDatabaseChatBot.Application.Common.Interfaces;
using QuoteDatabaseChatBot.Application.Common.Models;
using QuoteDatabaseChatBot.Domain.Entities;

namespace QuoteDatabaseChatBot.Application.Quotes.Queries {
    public class GetQuoteQuery : IRequest<IEnumerable<QuoteDto>> {
        public List<string> Contains { get; set; }
        public string? Game { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public QueryOutput? Output { get; set; }
        public bool MultipleResults { get; set; } = true;
        public bool CaseSensitive { get; set; } = false;
        public int LevenshteinDistanceThreshold { get; set; } = 0;

        public bool ArgsAreEmpty() {
            return (!Contains.Where(x => !string.IsNullOrWhiteSpace(x)).Any())
                && From == null
                && To == null
                && string.IsNullOrWhiteSpace(Game);
        }

        public GetQuoteQuery() {
            Contains = new List<string>();
        }
    }

    public class GetQuoteHandler : IRequestHandler<GetQuoteQuery, IEnumerable<QuoteDto>> {
        private readonly IQuoteCollectionDbContext _context;

        public GetQuoteHandler(IQuoteCollectionDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<QuoteDto>> Handle(GetQuoteQuery request, CancellationToken cancellationToken) {
            ExpressionStarter<Quote> predicate = PredicateBuilder.New<Quote>(true);
            if (request.Contains != null) {
                request.Contains.ForEach(quotePart => {
                    predicate = predicate.And(x => x.Content.ToLower().Contains(quotePart.ToLower()));
                });
            }

            if (!String.IsNullOrWhiteSpace(request.Game)) {
                predicate = predicate.And(x => x.Game.ToLower().Contains(request.Game.ToLower()));
            }

            if (request.From != null) {
                predicate = predicate.And(x => x.Date >= request.From);
            }

            if (request.To != null) {
                predicate = predicate.And(x => x.Date <= request.To);
            }

            return await _context.Quotes
                .AsExpandableEFCore()
                .Where(predicate)
                .Select(x => new QuoteDto {
                    QuoteId = x.Id,
                    QuoteContent = x.Content,
                    Game = x.Game,
                    Date = x.Date
                })
                .ToListAsync(cancellationToken);
        }
    }
}
