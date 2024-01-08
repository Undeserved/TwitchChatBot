using Microsoft.EntityFrameworkCore;
using QuoteDatabaseChatBot.Application.Common.Interfaces;
using QuoteDatabaseChatBot.Domain.Entities;
using QuoteDatabaseChatBot.Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Persistence {
    public class QuoteCollectionDbContext : DbContext, IQuoteCollectionDbContext {
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Wrapped_GameTotals> Wrapped_GameTotals { get; set; }
        public DbSet<Wrapped_Frequency> Wrapped_Frequencies { get; set; }
        public DbSet<Wrapped_GameOfTheMonth> Wrapped_GamesOfTheMonth { get; set; }
        public DbSet<Wrapped_MonthlyTotals> Wrapped_MonthlyTotals { get; set; }


        public QuoteCollectionDbContext(DbContextOptions<QuoteCollectionDbContext> options)
            : base(options) {
            base.Database.EnsureCreated();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuoteCollectionDbContext).Assembly);
            modelBuilder.Entity<Wrapped_GameTotals>().ToView(nameof(Wrapped_GameTotals));
            modelBuilder.Entity<Wrapped_Frequency>().ToView(nameof(Wrapped_Frequency));
            modelBuilder.Entity<Wrapped_GameOfTheMonth>().ToView(nameof(Wrapped_GameOfTheMonth));
            modelBuilder.Entity<Wrapped_MonthlyTotals>().ToView(nameof(Wrapped_MonthlyTotals));
        }
    }
}
