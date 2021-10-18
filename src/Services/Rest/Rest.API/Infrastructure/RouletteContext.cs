using Crosscuting.SeedWork.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Rest.API.Domain.AggregatesModel;
using Rest.API.Infrastructure.EntityTypeConfiguration;
using Rest.API.Infrastructure.Services.IdentityServices;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rest.API.Infrastructure
{
    public class RouletteContext : DbContext, IUnitOfWork
    {
        private readonly IIdentityServices _identityService;
        public static readonly string DEFAULT_SCHEMA = "dbo";

        public RouletteContext(
            DbContextOptions<RouletteContext> options,
            IIdentityServices identityService)
            : base(options)
        {
            _identityService = identityService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RouletteEntityTypeConfiguration());

        }

        public override int SaveChanges()
        {
            AuditEntities();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AuditEntities();
            var result = await base.SaveChangesAsync();

            return result;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var contextTransaction = await Database.BeginTransactionAsync(cancellationToken);
            return contextTransaction;
        }

        private void AuditEntities()
        {
            var currentUserName = _identityService.GetUserIdentity();
            var currentDateTime = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries())
            {
                const StringComparison currentCultureIgnoreCase = StringComparison.CurrentCultureIgnoreCase;

                if (entry.State == EntityState.Added)
                {
                    if (entry.Properties.Any(p => string.Equals(p.Metadata.Name, "UserRegister", currentCultureIgnoreCase)))
                    {
                        entry.Property("UserRegister").CurrentValue = currentUserName;
                    }
                    if (entry.Properties.Any(p => string.Equals(p.Metadata.Name, "DateRegister", currentCultureIgnoreCase)))
                    {
                        entry.Property("DateRegister").CurrentValue = currentDateTime;
                    }
                    if (entry.Properties.Any(p => string.Equals(p.Metadata.Name, "Activo", currentCultureIgnoreCase)))
                    {
                        entry.Property("Annulled").CurrentValue = false;
                    }
                }

                if (entry.State == EntityState.Modified)
                {
                    if (entry.Properties.Any(p => string.Equals(p.Metadata.Name, "UserModify", currentCultureIgnoreCase)))
                    {
                        entry.Property("UserModify").CurrentValue = currentUserName;
                    }
                    if (entry.Properties.Any(p => string.Equals(p.Metadata.Name, "DateModify", currentCultureIgnoreCase)))
                    {
                        entry.Property("DateModify").CurrentValue = currentDateTime;
                    }
                }
            }
        }


        public virtual DbSet<Roulette> Roulette { get; set; }
        public virtual DbSet<Board> Board { get; set; }
    }
}
