namespace CouchPartyGames.TournamentGenerator;

using CouchPartyGames.TournamentGenerator.Opponent;
using CouchPartyGames.TournamentGenerator.Type;

public class Tournament<TOpponent> {
    public enum Progression {
        Win,
        Lose
    }

    public required string Name { get; init; }

    public required bool AllowThirdPlace { get; init; }

    public required FinalsType FinalsType { get; init; }


    public List<TOpponent> ActiveOpponents { get; init; } = new();
/*    
    public List<IOpponent> Backlog { get; init; } = new();
    public List<IOpponent> RejectedOpponents { get; init; } = new();
*/

    public List<Match<TOpponent>> Matches { get; init; } = new();

    public Tournament() { }

    public Match<TOpponent> GetMatch(int localMatchId) {
        //return Matches.Where(m => m.LocalMatchId == localMatchId).First();
        throw new NotImplementedException();
    }

    // <summary>
    // Find the next match
    public Match<TOpponent> GetNextMatch(int localMatchId, Progression progression) {
        /*
        var nextMatchId = Matches.Where(m => m.LocalMatchId == localMatchId).Select(m => m.WinProgression).FirstOrDefault();
        if (nextMatchId is null) {
            return NotFound;
        } else if (nextMatchId == -1) {
            return NoMatch;
        } */

        /*
        return Matches
            .Where(m => m.LocalMatchId == localMatchId)
            .Select(m => m.WinProgression)
            .First();
            */
        throw new NotImplementedException();
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
