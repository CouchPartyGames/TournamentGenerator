namespace CouchPartyGames.TournamentGenerator.Position;

public interface IOpponentStartPosition {

    public DrawSize DrawSize { get; }

    public List<OpponentStartPosition> Matches { get; }
}
