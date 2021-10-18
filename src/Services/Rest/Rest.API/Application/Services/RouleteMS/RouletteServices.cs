using Crosscuting.Common.Util;
using Rest.API.Application.Adapters.RouletteDTOs;
using Rest.API.Domain.AggregatesModel;
using Rest.API.Infrastructure.Exceptions;
using Rest.API.Infrastructure.Repositories.BoardRepository;
using Rest.API.Infrastructure.Repositories.RouletteRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest.API.Application.Services.RouleteMS
{
    public class RouletteServices: IRouletteServices
    {
        #region Variables

        private readonly IRouletteRepository _repository;
        private readonly IBoardRepository _boardRepository;

        #endregion

        #region Builder

        public RouletteServices(IRouletteRepository repository, IBoardRepository boardRepository)
        {
            _repository = repository;
            _boardRepository = boardRepository;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<RouletteListDTO>> ListAsync()
        {
            var rouletteList = (await _repository.FindByAsync(item => !item.Annulled))
                .Select(entity => new RouletteListDTO()
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    Opened = entity.Opened,
                    Bets = new List<BoardBetListDTO>()
                }).ToList();

            foreach (var board in rouletteList)
            {
                board.Bets = (await _boardRepository.FindByAsync(p => !p.Annulled && p.RouletteId == board.Id))
                        .Select(bet => new BoardBetListDTO()
                        {
                            PlayerId = bet.PlayerId,
                            NumberBet = bet.NumberBet,
                            MoneyBet = bet.MoneyBet,
                            NumberWinning = bet.NumberWinning,
                            MoneyEarned = bet.MoneyEarned
                        }).ToList();
            }

            return rouletteList;
        }

        public async Task<Roulette> AddAsync(RouletteRegisterDTO item)
        {
            var count = await _repository.CountAsync(p => !p.Annulled && p.Description == item.Description );

            if (count > 0)
            {
                throw new RouletteDomainException(Constants.MsgRepeatedElement);
            }

            var entity = new Roulette()
            {
                Description = item.Description
            };

            await _repository.AddAsync(entity);
            await _repository.UnitOfWork.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> OpenAsync(int id)
        {
            if (id == default)
            {
                throw new RouletteDomainException(Constants.MsgKeyNotFound);
            }

            var count = await _repository.CountAsync(item => item.Id == id && !item.Annulled);

            if (count == 0)
            {
                throw new RouletteDomainException(Constants.MsgDataNotFound);
            }

            var countOpened = await _repository.CountAsync(item => item.Id == id && !item.Annulled && item.Opened == true);

            if (countOpened > 0)
            {
                throw new RouletteDomainException("La ruleta ya está abierta.");
            }
            var countClosed = await _repository.CountAsync(item => item.Id == id && !item.Annulled && item.Opened == false);

            if (countClosed > 0)
            {
                throw new RouletteDomainException("La ruleta ya está cerrada.");
            }

            var entity = await _repository.GetByIdAsync(id);

            entity.Opened = true;

            await _repository.ModifyAsync(entity);
            await _repository.UnitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<Board> BetAsync( RouletteBetRegisterDTO item, int playerId)
        {
            if (playerId == default)
            {
                throw new RouletteDomainException(Constants.MsgKeyNotFound);
            }

            var count = await _boardRepository.CountAsync(p => !p.Annulled && p.PlayerId == playerId && p.RouletteId == item.RouletteId);

            if (count > 0)
            {
                throw new RouletteDomainException("El jugador ya ha realizado una apuesta.");
            }
            if( item.NumberBet < 0 || item.NumberBet > 38)
            {
                throw new RouletteDomainException("Número de apuesta invalido.");
            }
            if (item.MoneyBet < 0 || item.MoneyBet > 10000)
            {
                throw new RouletteDomainException("Dinero de apuesta invalido.");
            }

            var countNotOpened = await _repository.CountAsync(cop => cop.Id == item.RouletteId && !cop.Annulled && cop.Opened != true);

            if (countNotOpened > 0)
            {
                throw new RouletteDomainException("La ruleta no está abierta.");
            }

            var entity = new Board()
            {
                RouletteId = item.RouletteId,
                PlayerId = playerId,
                NumberBet = item.NumberBet,
                MoneyBet = item.MoneyBet
            };

            await _boardRepository.AddAsync(entity);
            await _boardRepository.UnitOfWork.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Board>> CloseAsync(int id)
        {
            if (id == default)
            {
                throw new RouletteDomainException(Constants.MsgKeyNotFound);
            }

            var count = await _repository.CountAsync(item => item.Id == id && !item.Annulled);

            if (count == 0)
            {
                throw new RouletteDomainException(Constants.MsgDataNotFound);
            }

            var countOpened = await _repository.CountAsync(item => item.Id == id && !item.Annulled && item.Opened == true);

            if (countOpened == 0)
            {
                throw new RouletteDomainException("La ruleta no está abierta.");
            }
            var countClosed = await _repository.CountAsync(item => item.Id == id && !item.Annulled && item.Opened == false);

            if (countClosed > 0)
            {
                throw new RouletteDomainException("La ruleta ya está cerrada.");
            }

            var _numberWinning = new Random().Next(0, 36);

            var boardBets = await _boardRepository.FindByAsync(p => p.RouletteId == id);

            

            foreach (var boardItem in boardBets)
            {
                boardItem.NumberWinning = _numberWinning;
                if(boardItem.NumberBet == 37)
                {
                    boardItem.MoneyEarned = _numberWinning % 2 == 0 ? boardItem.MoneyBet * Convert.ToDecimal(1.80) : 0 ;
                }else if (boardItem.NumberBet == 38)
                {
                    boardItem.MoneyEarned = _numberWinning % 2 != 0 ? boardItem.MoneyBet * Convert.ToDecimal(1.80) : 0;
                }
                else if(boardItem.NumberBet == _numberWinning)
                {
                    boardItem.MoneyEarned = boardItem.MoneyBet * 5;
                }
                else
                {
                    boardItem.MoneyEarned = 0;
                }

                await _boardRepository.ModifyAsync(boardItem);
            }

            await _boardRepository.UnitOfWork.SaveChangesAsync();

            var entity = await _repository.GetByIdAsync(id);

            entity.Opened = false;

            await _repository.ModifyAsync(entity);
            await _repository.UnitOfWork.SaveChangesAsync();

            return boardBets;
        }
        #endregion
    }
}
