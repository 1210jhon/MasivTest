using System.Collections.Generic;

namespace Rest.API.Application.Adapters.RouletteDTOs
{
    public class RouletteListDTO
    {
        public RouletteListDTO()
        {
            Bets = new List<BoardBetListDTO>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? Opened { get; set; }

        public List<BoardBetListDTO> Bets { get; set; }
    }

    public class BoardBetListDTO
    {
        public int PlayerId { get; set; }
        public int NumberBet { get; set; }
        public decimal MoneyBet { get; set; }
        public int? NumberWinning { get; set; }
        public decimal? MoneyEarned { get; set; }
    }
}
