
namespace CouchPartyGames.TournamentGenerator.Type;

using CouchPartyGames.TournamentGenerator.Position;
using CouchPartyGames.TournamentGenerator;
using CouchPartyGames.TournamentGenerator.Exceptions;


internal sealed class CreateMatchProgression
{

    private readonly IOpponentStartPosition _positions;

    public List<MatchProgression> Matches { get; private set; } = new();

    private TournamentFinals _finalsType;

    private readonly int _totalRounds;

    public CreateMatchProgression(IOpponentStartPosition positions, 
        TournamentFinals finalsType,
        Tournament3rdPlace tournament3rdPlace)
    {
        _positions = positions;
        _finalsType = finalsType;
        _totalRounds = positions.DrawSize.ToTotalRounds();
        Create();
    }


    void Create()
    {
        int round = 1;
        int matchId = 1;
        int thirdPlaceMatchId = 99;
        int totalMatchesInRound = GetTotalMatchesInRound(round); 

        Create1stRound(totalMatchesInRound);
        matchId = totalMatchesInRound + 1;

        for(round = 2; round < _totalRounds; round++) {
            var prevRoundMatches = GetPreviousRoundMatches(round);
            var chunkPairs = prevRoundMatches.Chunk<MatchProgression>(2);

            totalMatchesInRound = GetTotalMatchesInRound(round);
            if (chunkPairs.ToList().Count != totalMatchesInRound) {
                throw new MismatchProgressionChunkSize("Mismatching number of previous matches");
            }

            foreach(var pair in chunkPairs) {
                Matches.Add(MatchProgression.CreateOtherRounds(round, matchId));

                    // Update previous matches with proper progression
                foreach(var prevMatch in pair) {
                    prevMatch.UpdateWinProgression(matchId);
                }
                matchId ++;
            }
        }

        CreateThirdPlace(_totalRounds - 1, thirdPlaceMatchId);
        CreateFinalRounds(_totalRounds, matchId);
    }


    void Create1stRound(int numMatches, int round = 1) {

        for(int i = 1; i <= numMatches; i++) {
            Matches.Add(MatchProgression.Create1stRound(round, i, _positions.Matches[i]));
	    }
    }

    void CreateThirdPlace(int semiFinalsRound, int thirdPlaceMatchId) {
        int round = 99;

        Matches.Add(MatchProgression.CreateOtherRounds(round, thirdPlaceMatchId));

            // Add Lose Progression to Semifinals matches
        var semiFinalsMatches = Matches.Where(m => m.Round == semiFinalsRound).ToList();
        if (semiFinalsMatches.Count != 2) {
            throw new ArgumentException("Semifinals should have only 2 matches");
        }
        semiFinalsMatches.ForEach(x => {
            x.UpdateLoseProgression(thirdPlaceMatchId);
        });
    }

    void CreateFinalRounds(int matchId, int round) {

        switch (_finalsType)
        {
            case TournamentFinals.OneOfOne:
                Matches.Add(MatchProgression.CreateOtherRounds(round, matchId));

                int semiFinalsRound = round - 1; 
                // Add Win Progression to Semifinals
                var semiFinalsMatches = Matches.Where(m => m.Round == semiFinalsRound).ToList();
                if (semiFinalsMatches.Count != 2) {
                    throw new ArgumentException("Semifinals should have only 2 matches");
                }
                semiFinalsMatches.ForEach(x => {
                    x.UpdateWinProgression(matchId);
                });

                break;

            case TournamentFinals.TwoOfThree:
                Matches.Add(MatchProgression.CreateOtherRounds(round, matchId));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 1, matchId + 1));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 2, matchId + 2));
                break;

            case TournamentFinals.ThreeOfFive:
                Matches.Add(MatchProgression.CreateOtherRounds(round, matchId));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 1, matchId + 1));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 2, matchId + 2));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 3, matchId + 3));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 4, matchId + 4));
                break;
        }
    }

    List<MatchProgression> GetPreviousRoundMatches(int curRound) {
        int prevRound = curRound -1;
        return Matches
            .Where(m => m.Round == prevRound)
            .ToList();
    }

    int GetTotalMatchesInRound(int round) => (int)_positions.DrawSize.Value / (int)Math.Pow(2, round);
}