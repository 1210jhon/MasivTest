using Crosscuting.SeedWork.Domain;

namespace Rest.API.Domain.AggregatesModel
{
    public class Roulette : Entity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? Opened { get; set; }
    }
}
