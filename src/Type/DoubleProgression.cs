namespace CouchPartyGames.TournamentGenerator.Type;

using CouchPartyGames.TournamentGenerator.Position;
using CouchPartyGames.TournamentGenerator.Exceptions;

public sealed class DoubleProgression {


    private readonly IOpponentStartPosition _positions;

    public List<MatchProgression> WinnersBracketMatches { get; private set; } = new();
    public List<MatchProgression> Matches { get; private set; } = new();

    private readonly int _totalRounds;

    private int _matchId = 1;

    private int _losingRound = 101;

    public DoubleProgression(IOpponentStartPosition positions) {
        _positions = positions;
        _totalRounds = positions.DrawSize.ToTotalRounds();
        Create();
    }

    void Create() {

        bool gatherMatchesFromWinnersBracket = true;

        int round = 1; 
        int totalMatchesInRound = GetTotalMatchesInRound(round);

        Create1stRoundWinnersBracket(totalMatchesInRound);
        Create1stRoundLosersBracket();

        for(round = 2; round < _totalRounds; round++) {
            AddNextRound(round);
        }
        //AddFinalsRound();
    }


    void Create1stRoundWinnersBracket(int numMatches, int round = 1) {
        if (numMatches != _positions.Matches.Count) {
            throw new InvalidFirstRoundMatchesException("Draw size doesn't match number of first round matches");
        }

        for (int i = 1; i <= numMatches; i++) {
            Matches
                .Add(MatchProgression.Create1stRound(round, _matchId, _positions.Matches[i - 1]));
            _matchId++;
        }
    }

    /*
    Add 1st Round Matches for the Losers Bracket
    Update the Lose progression 
    */
    void Create1stRoundLosersBracket() {
            // Update Progression (Lose) of Winners First Round

        foreach(var pair in GetRoundMatchesInChunks(1)) {
            Matches.Add(MatchProgression.CreateOtherRounds(_losingRound, _matchId));

            foreach(var prevFirstRoundMatch in pair) {
                    // update winners bracket
                prevFirstRoundMatch.UpdateLoseProgression(_matchId);
            }
            _matchId++;
        }
        _losingRound++;
    }


    void AddNextRound(int curRound) {

            // Get Previous Round Matches
            // Create Pair(s) of 2 
        foreach(var pair in GetRoundMatchesInChunks(curRound - 1)) {

                // Add Current Round Match
            Matches.Add(MatchProgression.CreateOtherRounds(curRound, _matchId));

                // Update Previous Progression (Win Progression)
            foreach(var prevMatch in pair) {
                prevMatch.UpdateWinProgression(_matchId);
            }

            _matchId++;
        }

        AddLosingRound(curRound);
    }

    void AddLosingRound(int curWinRound) {
        var chunkPrevLosers = GetRoundMatchesInChunks(_losingRound - 1);
        var chunkCurWinners = GetRoundMatchesInChunks(curWinRound);


        /*
        if (chunkPrevLosers.Count != chunkCurWinners.Count) {
            throw new Exception("Mismatching");
        }*/
        int numMatches = chunkPrevLosers.ToList().Count;

            // Create Matches
        for (int i = 0; i < numMatches; i++) {
            Matches.Add(MatchProgression.CreateOtherRounds(_losingRound, _matchId));
            _matchId++;
        }
        _losingRound++;

            // Create Next Matches
            // Previous Round winners will play each other
        for(int i = 0; i < numMatches / 2; i++) {
            Matches.Add(MatchProgression.CreateOtherRounds(_losingRound, _matchId));
            _matchId++;
        }
        _losingRound++;

    }

    void AddFinalsRound(int finalsRound) {
            // Add Finals Match in Winners Bracket
            // ToDo - Add Progression
        Matches.Add(MatchProgression.CreateOtherRounds(finalsRound, _matchId));
        _matchId++;

            // Add Finals Match in Losers Bracket
            // ToDo - Add Progression
        Matches.Add(MatchProgression.CreateOtherRounds(_losingRound, _matchId));
        _matchId++;   

            // Final #2 Match
            // ToDo - Add Progression
        Matches.Add(MatchProgression.CreateOtherRounds(optionalRound, _matchId));
        _matchId++;   
        
            // Final #3 - Each Player has 1 loss
            // Optional Match
        Matches.Add(MatchProgression.CreateOtherRounds(optionalRound, _matchId));
        _matchId++;   
    }


    IEnumerable<MatchProgression[]> GetRoundMatchesInChunks(int round) => 
        Matches
            .Where(x => x.Round == round)
            .ToList()
            .Chunk<MatchProgression>(2);

    void AddMatch(MatchProgression match) {
        Matches.Add(match);
        _matchId++;
    }

    int GetTotalMatchesInRound(int round) => (int)_positions.DrawSize.Value / (int)Math.Pow(2, round);

}