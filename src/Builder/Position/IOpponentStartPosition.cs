namespace CouchPartyGames.TournamentGenerator.Builder.Position;

using CouchPartyGames.TournamentGenerator.Builder.Size;

public interface IOpponentStartPosition {

    public DrawSize DrawSize { get; }

    public List<OpponentStartPosition> Matches { get; }
}
