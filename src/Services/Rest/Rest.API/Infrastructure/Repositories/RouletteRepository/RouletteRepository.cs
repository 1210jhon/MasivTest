using Crosscuting.SeedWork.Infrastructure;
using Rest.API.Application.Adapters.RouletteDTOs;
using Rest.API.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rest.API.Infrastructure.Repositories.RouletteRepository
{
    public class RouletteRepository : Repository<Roulette>, IRouletteRepository
    {
        #region Variables

        private readonly RouletteContext _ctx;

        #endregion

        #region Builder

        public RouletteRepository(RouletteContext context) : base(context: context) => _ctx = context ?? throw new ArgumentNullException(nameof(context));

        #endregion

        #region Methods
        #endregion
    }
}
