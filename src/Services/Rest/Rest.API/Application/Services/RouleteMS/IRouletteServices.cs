using Rest.API.Application.Adapters.RouletteDTOs;
using Rest.API.Domain.AggregatesModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rest.API.Application.Services.RouleteMS
{
    public interface IRouletteServices
    {
        Task<IEnumerable<RouletteListDTO>> ListAsync();
        Task<Roulette> AddAsync(RouletteRegisterDTO item);
        Task<bool> OpenAsync(int rouletteId);
        Task<Board> BetAsync(RouletteBetRegisterDTO item, int playerId);
        Task<IEnumerable<Board>> CloseAsync(int id);
    }
}
