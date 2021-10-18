using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rest.API.Domain.AggregatesModel;
namespace Rest.API.Infrastructure.EntityTypeConfiguration
{
    public class RouletteEntityTypeConfiguration: IEntityTypeConfiguration<Roulette>
    {
        public void Configure(EntityTypeBuilder<Roulette> builder)
        {
            builder.ToTable(name: "Roulette", RouletteContext.DEFAULT_SCHEMA);

            builder.HasKey(item => item.Id);
        }
    }
}
