namespace CouchPartyGames.TournamentGenerator.Opponent;

public interface IOpponent {

    string Name { get; init; }

    int Rank { get; init; }
}