using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rest.API.Domain.AggregatesModel;

namespace Rest.API.Infrastructure.EntityTypeConfiguration
{
    public class BoardEntityTypeConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.ToTable(name: "Board", RouletteContext.DEFAULT_SCHEMA);

            builder.HasKey(item => item.Id);
        }
    }
}
