namespace CouchPartyGames.TournamentGenerator.Type;

public sealed record Match<TOpponent> {

    const int NoProgression = -1;
    const int NoPosition = -1;

    public int LocalMatchId { get; set; }

    public int Round { get; set; }

    public int WinProgression { get; set; } = NoProgression;

    public int LoseProgression { get; set; } = NoProgression;

    public TOpponent Opponent1 { get; set; }

    public TOpponent Opponent2 { get; set; }

    public int Opponent1Position { get; init; } = NoPosition;

    public int Opponent2Position { get; init;} = NoPosition;


    public static Match<TOpponent> NewNoProgression(int localMatchId, int round) =>
        new Match<TOpponent> {
            LocalMatchId = localMatchId,
            Round = round,
            WinProgression = NoProgression,
            LoseProgression = NoProgression
        };


	public static Match<TOpponent> New(int localMatchId, int round, int winProgression) =>
        new Match<TOpponent> {
            LocalMatchId = localMatchId,
            Round = round,
            WinProgression = winProgression,
            LoseProgression = NoProgression
        };


    public static Match<TOpponent> New(MatchProgression matchProgression) => 
        new Match<TOpponent> {
            LocalMatchId = matchProgression.LocalMatchId,
            Round = matchProgression.Round,
            Opponent1Position = matchProgression.Position1,
            Opponent2Position = matchProgression.Position2,
            WinProgression = matchProgression.WinProgressionMatchId,
            LoseProgression = matchProgression.LoseProgressionMatchId
        };
    
    public static Match<TOpponent> New(MatchProgression matchProgression, TOpponent opponent1, TOpponent opponent2) => 
        new Match<TOpponent> {
            LocalMatchId = matchProgression.LocalMatchId,
            Round = matchProgression.Round,
            Opponent1 = opponent1,
            Opponent2 = opponent2,
            Opponent1Position = matchProgression.Position1,
            Opponent2Position = matchProgression.Position2,
            WinProgression = matchProgression.WinProgressionMatchId,
            LoseProgression = matchProgression.LoseProgressionMatchId
        };

    public bool NextWinProgressionExists() => WinProgression != NoProgression;
    public bool NextLoseProgressionExists() => LoseProgression != NoProgression;
}
