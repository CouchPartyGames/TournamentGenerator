namespace CouchPartyGames.TournamentGenerator.Type;

using CouchPartyGames.TournamentGenerator.Position;

public sealed class CreateDoubleProgression {


    private readonly IOpponentStartPosition _positions;

    public List<MatchProgression> WinnersBracketMatches { get; private set; } = new();
    public List<MatchProgression> LosersBracketMatches { get; private set; } = new();

    private readonly int _totalRounds;

    public CreateDoubleProgression(IOpponentStartPosition positions) {
        _positions = positions;
        _totalRounds = positions.DrawSize.ToTotalRounds();
    }

    void Create() {

        bool gatherMatchesFromWinnersBracket = true;

        int round = 1; 
        int matchId = 1;
        int loserMatchId = 1001;
        int totalMatchesInRound = GetTotalMatchesInRound(round);

        Create1stRoundWinnersBracket(totalMatchesInRound);
        Create1stRoundLosersBracket(totalMatchesInRound / 2);

        for(round = 2; round <= _totalRounds; round++) {

            if (gatherMatchesFromWinnersBracket) {

                //CreateNthRoundWinners();
                //CreateNthRoundLosers();
                gatherMatchesFromWinnersBracket = false;
            } 

            //CreateNthRoundLosers();

        }
    }


    void Create1stRoundWinnersBracket(int numMatches, int round = 1) {
        for (int i = 1; i <= numMatches; i++) {
            WinnersBracketMatches
                .Add(MatchProgression.Create1stRound(round, i, _positions.Matches[i]));
        }
    }

    /*
    Add 1st Round Matches for the Losers Bracket
    Update the Lose progression 
    */
    void Create1stRoundLosersBracket(int numMatches) {
        int round = 99;
        int matchId = 99;
            // Create 1st Round of Loser's Bracket
        for (int i = 1; i <= numMatches; i++) {
            LosersBracketMatches.Add(MatchProgression.CreateOtherRounds(round, matchId));
            matchId++;
        }
            // Update Progression (Lose) of Winners First Round
        var firstRound = WinnersBracketMatches
            .Where(x => x.Round == 1)
            .ToList();

        int loserBracketMatchId = 99;
        var chunkPairs = firstRound.Chunk<MatchProgression>(2);
        foreach(var pair in chunkPairs) {

            foreach(var m in pair) {
                    // update winners bracket
                m.UpdateLoseProgression(loserBracketMatchId);
            }
            loserBracketMatchId++;
        }
        
    }

    int GetTotalMatchesInRound(int round) => (int)_positions.DrawSize.Value / (int)Math.Pow(2, round);

}