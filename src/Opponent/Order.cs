namespace CouchPartyGames.TournamentGenerator.Opponent;

using CouchPartyGames.TournamentGenerator.Exceptions;

public abstract class Order
{
    protected const int _startIndex = 1;

    protected const int MinParticipants = 2;

    public enum OrderType
    {
        Random = 0,
        Ranked
    };

    public abstract Dictionary<int, IOpponent> Opponents { get; }

    public static Order Create(OrderType order, List<IOpponent> opponents) => order switch
    {
        OrderType.Ranked => new RankedOrder(opponents),
        _ => new RandomOrder(opponents)
    };
}
