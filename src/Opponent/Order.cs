namespace CouchPartyGames.TournamentGenerator.Opponent;

using CouchPartyGames.TournamentGenerator;
using CouchPartyGames.TournamentGenerator.Exceptions;

public abstract class Order<TOpponent> where TOpponent : IOpponent
{
    protected const int _startIndex = 1;

    protected const int MinParticipants = 2;

    public enum OrderType
    {
        Random = 0,
        Ranked
    };

    public abstract Dictionary<int, TOpponent> Opponents { get; }

    public static Order<TOpponent> Create(OrderType order, List<TOpponent> opponents) => order switch
    {
        OrderType.Ranked => new RankedOrder<TOpponent>(opponents),
        _ => new RandomOrder<TOpponent>(opponents)
    };

    public static Order<TOpponent> Create(TournamentSeeding order, List<TOpponent> opponents) => order switch {
        TournamentSeeding.Ranked => new RankedOrder<TOpponent>(opponents),
        _ => new RandomOrder<TOpponent>(opponents)
    };
}
