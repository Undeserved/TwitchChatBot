using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuoteDatabaseChatBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Persistence.Configurations {
    public class QuotesConfiguration : IEntityTypeConfiguration<Quote> {
        public void Configure(EntityTypeBuilder<Quote> builder) {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired();
            builder.Property(x => x.Content)
                .IsRequired();
            builder.Property(x => x.Date)
                .IsRequired();
            builder.Property(x => x.Game)
                .IsRequired();
        }
    }
}
