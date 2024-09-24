namespace CouchPartyGames.TournamentGenerator.Builder.Type;

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

    public static Match<TOpponent> New(MatchWithId matchWithId, int winProgression) =>
        new Match<TOpponent> {
            LocalMatchId = matchWithId.LocalMatchId,
            Round = matchWithId.Round,
            WinProgression = winProgression,
            Opponent1Position = matchWithId.Position1,
            Opponent2Position = matchWithId.Position2
        };
}
