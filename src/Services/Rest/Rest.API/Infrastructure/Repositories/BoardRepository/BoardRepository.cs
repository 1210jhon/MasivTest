using Crosscuting.SeedWork.Infrastructure;
using Rest.API.Domain.AggregatesModel;
using System;

namespace Rest.API.Infrastructure.Repositories.BoardRepository
{
    public class BoardRepository : Repository<Board>, IBoardRepository
    {
        #region Variables

        private readonly RouletteContext _ctx;

        #endregion

        #region Builder

        public BoardRepository(RouletteContext context) : base(context: context) => _ctx = context ?? throw new ArgumentNullException(nameof(context));

        #endregion

        #region Methods



        #endregion
    }
}
