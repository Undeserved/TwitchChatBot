using MediatR;
using QuoteDatabaseChatBot.Application.Common.Dtos.Wrapped;
using QuoteDatabaseChatBot.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Application.Wrapped.Queries {
    public class GameTotalsQuery : IRequest<IEnumerable<GameTotalsDto>> {
        public int Year { get; set; }
        public bool Filter { get; set; } = false;
    }

    public class GameTotalsHandler : IRequestHandler<GameTotalsQuery, IEnumerable<GameTotalsDto>> {
        private readonly IQuoteCollectionDbContext _context;

        public GameTotalsHandler(IQuoteCollectionDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<GameTotalsDto>> Handle(GameTotalsQuery request, CancellationToken cancellationToken) {
            return _context.Wrapped_GameTotals
                .Where(x => x.Year >= (request.Filter ? request.Year : x.Year))
                .Select(x => new GameTotalsDto {
                    Game = x.Game,
                    Year = x.Year,
                    Count = x.Count
                });
        }
    }
}
