using Microsoft.EntityFrameworkCore;
using QuoteDatabaseChatBot.Domain.Entities;
using QuoteDatabaseChatBot.Domain.Views;

namespace QuoteDatabaseChatBot.Application.Common.Interfaces {
    public interface IQuoteCollectionDbContext {
        DbSet<Quote> Quotes { get; set; }
        DbSet<Wrapped_GameTotals> Wrapped_GameTotals { get; set; }
        DbSet<Wrapped_Frequency> Wrapped_Frequencies { get; set; }
        DbSet<Wrapped_GameOfTheMonth> Wrapped_GamesOfTheMonth { get; set; }
        DbSet<Wrapped_MonthlyTotals> Wrapped_MonthlyTotals { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
