
namespace CouchPartyGames.TournamentGenerator.Type;

using CouchPartyGames.TournamentGenerator.Position;
using CouchPartyGames.TournamentGenerator;
using CouchPartyGames.TournamentGenerator.Exceptions;


internal sealed class SingleProgression
{

    private readonly IOpponentStartPosition _positions;

    public List<MatchProgression> Matches { get; private set; } = new();

    private TournamentFinals _finalsType;

    private readonly Tournament3rdPlace _thirdPlace;

    private readonly int _totalRounds;

    public SingleProgression(IOpponentStartPosition positions, 
        TournamentFinals finalsType,
        Tournament3rdPlace tournament3rdPlace)
    {
        _positions = positions;
        _finalsType = finalsType;
        _thirdPlace = tournament3rdPlace;
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
            var chunkPairs = GetRoundMatchesInChunks(round - 1);
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

        if (_thirdPlace == Tournament3rdPlace.ThirdPlace) {
            CreateThirdPlace(_totalRounds - 1, thirdPlaceMatchId);
        }
        CreateFinalRounds(_totalRounds, matchId);
    }


    void Create1stRound(int numMatches, int round = 1) {
        if (numMatches != _positions.Matches.Count) {
            throw new InvalidFirstRoundMatchesException("Draw size doesn't match number of first round matches");
        }

        for(int i = 1; i <= numMatches; i++) {
            Matches.Add(MatchProgression.Create1stRound(round, i, _positions.Matches[i - 1]));
	    }
    }

    void CreateThirdPlace(int semiFinalsRound, int thirdPlaceMatchId = 99) {
        int round = 99;

            // Create 3rd Place Match
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

    void CreateFinalRounds(int round, int matchId) {

        int semiFinalsRound = round - 1; 
        switch (_finalsType)
        {
            case TournamentFinals.OneOfOne:
                Matches.Add(MatchProgression.CreateOtherRounds(round, matchId));
                break;

            case TournamentFinals.TwoOfThree:
                Matches.Add(MatchProgression.CreateOtherRounds(round, matchId, matchId + 1));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 1, matchId + 1, matchId + 2));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 2, matchId + 2));
                break;

            case TournamentFinals.ThreeOfFive:
                Matches.Add(MatchProgression.CreateOtherRounds(round, matchId, matchId + 1));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 1, matchId + 1, matchId + 2));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 2, matchId + 2, matchId + 3));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 3, matchId + 3, matchId + 4));
                Matches.Add(MatchProgression.CreateOtherRounds(round + 4, matchId + 4));
                break;
        }

        UpdateSemifinalsWinProgression(semiFinalsRound, matchId);
    }

    void UpdateSemifinalsWinProgression(int semiFinalsRound, int finalsMatchId) {
        var semiFinalsMatches = Matches
            .Where(m => m.Round == semiFinalsRound)
            .ToList();

        if (semiFinalsMatches.Count != 2) {
            throw new ArgumentException("Semifinals should have only 2 matches");
        }

        semiFinalsMatches.ForEach(x => {
            x.UpdateWinProgression(finalsMatchId);
        });
    }

    List<MatchProgression> GetPreviousRoundMatches(int curRound) => 
        Matches
            .Where(m => m.Round == curRound - 1)
            .ToList();

    List<MatchProgression> GetRoundMatches(int round) => 
        Matches
            .Where(m => m.Round == round)
            .ToList();

    IEnumerable<MatchProgression[]> GetRoundMatchesInChunks(int round) =>
        Matches
            .Where(m => m.Round == round)
            .ToList()
            .Chunk<MatchProgression>(2);

    int GetTotalMatchesInRound(int round) => (int)_positions.DrawSize.Value / (int)Math.Pow(2, round);
}