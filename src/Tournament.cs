namespace CouchPartyGames.TournamentGenerator;

using CouchPartyGames.TournamentGenerator.Opponent;
using CouchPartyGames.TournamentGenerator.Type;

public class Tournament<TOpponent> 
    where TOpponent : IOpponent, IEquatable<TOpponent>
{

    public required string Name { get; init; }

    public required bool HasThirdPlace { get; init; }
    
    public int ThirdPlaceMatchId { get; init; } = -1;

    public required TournamentFinals FinalsType { get; init; }

    public required int Size { get; init; }

    public required string Seeding { get; init; }

    public List<TOpponent> ActiveOpponents { get; init; } = new();
    
    public List<IOpponent> RejectedOpponents { get; init; } = new();

    public List<Match<TOpponent>> Matches { get; init; } = new();
    
    public int FinalMatchId { get; init; } = -1;

    public Tournament() { }

    public Match<TOpponent>? GetMatch(int localMatchId) {
        return Matches.FirstOrDefault(m => m.LocalMatchId == localMatchId);
    }

    // <summary>
    // Find the next match
    public Match<TOpponent>? GetWinProgressionMatch(int localMatchId) {
        return Matches
            .Where(m => m.LocalMatchId == localMatchId)
            .Select(m => Matches.FirstOrDefault(m2 => m2.LocalMatchId == m.WinProgression))
            .FirstOrDefault();
    }

    public Match<TOpponent>? GetLoseProgressionMatch(int localMatchId)
    {
        return Matches.Where(m => m.LocalMatchId == localMatchId)
            .Select(m => Matches.FirstOrDefault(m2 => m2.LocalMatchId == m.LoseProgression))
            .FirstOrDefault();
    }
    
    // <summary>
    /*
    public Match GetNextMatch(int localMatchId, IOpponent opponent) =>
        Matches.Where(m => m.LocalMatchId == matches
            .Where(m => m.LocalMatchId == localMatchId)
            .Select(m => m.Progression)
            .First()
        ).First();

*/
/*
    public void UpdateMatch() {
        var match = Matches.Where(x => x.LocalMatchId == localMatchId).First();
    }
    */

}
