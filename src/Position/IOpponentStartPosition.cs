namespace CouchPartyGames.TournamentGenerator.Position;

using CouchPartyGames.TournamentGenerator.Size;

public interface IOpponentStartPosition {

    public DrawSize DrawSize { get; }

    public List<OpponentStartPosition> Matches { get; }
}
