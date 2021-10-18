using Crosscuting.SeedWork.Domain;

namespace Rest.API.Domain.AggregatesModel
{
    public class Board : Entity
    {
        public int Id { get; set; }
        public int RouletteId { get; set; }
        public int PlayerId { get; set; }
        public int NumberBet { get; set; }
        public decimal MoneyBet {get;set;}
        public int? NumberWinning { get; set; }
        public decimal? MoneyEarned { get; set; }
    }
}
