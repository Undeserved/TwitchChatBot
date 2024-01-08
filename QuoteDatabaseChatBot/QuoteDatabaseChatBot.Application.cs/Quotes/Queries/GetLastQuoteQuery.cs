using MediatR;
using Microsoft.EntityFrameworkCore;
using QuoteDatabaseChatBot.Application.Common.Interfaces;

namespace QuoteDatabaseChatBot.Application.Quotes.Queries {
    public class GetLastQuoteQuery : IRequest<int> {
    }

    public class GetLastQuoteHandler : IRequestHandler<GetLastQuoteQuery, int> {
        private readonly IQuoteCollectionDbContext _context;

        public GetLastQuoteHandler(IQuoteCollectionDbContext context) {
            _context = context;
        }

        public async Task<int> Handle(GetLastQuoteQuery request, CancellationToken cancellationToken) {
            return await _context.Quotes.MaxAsync(x => x.Id);
        }
    }
}
