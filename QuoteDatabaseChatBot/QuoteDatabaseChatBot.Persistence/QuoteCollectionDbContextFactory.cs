using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Persistence {
    public class QuoteCollectionDbContextFactory : DesignTimeDbContextFactoryBase<QuoteCollectionDbContext> {
        protected override QuoteCollectionDbContext CreateNewInstance(DbContextOptions<QuoteCollectionDbContext> options) {
            return new QuoteCollectionDbContext(options);
        }
    }
}
