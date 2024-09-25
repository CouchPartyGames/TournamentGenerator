namespace CouchPartyGames.TournamentGenerator;

using CouchPartyGames.TournamentGenerator.Opponent;
using CouchPartyGames.TournamentGenerator.Type;

public class Tournament<TOpponent> 
    where TOpponent : IOpponent 
{

    public required string Name { get; init; }

    public required string ThirdPlace { get; init; }

    public required TournamentFinals FinalsType { get; init; }

    public required int Size { get; init; }

    public required string Seeding { get; init; }

    public List<TOpponent> ActiveOpponents { get; init; } = new();
    
    public List<IOpponent> RejectedOpponents { get; init; } = new();

    public List<Match<TOpponent>> Matches { get; init; } = new();

    public Tournament() { }

    public Match<TOpponent> GetMatch(int localMatchId) {
        //return Matches.Where(m => m.LocalMatchId == localMatchId).First();
        throw new NotImplementedException();
    }

    // <summary>
    // Find the next match
    public Match<TOpponent> GetNextMatch(int localMatchId) {
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
