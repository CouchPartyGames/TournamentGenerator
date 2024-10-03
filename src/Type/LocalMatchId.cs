namespace CouchPartyGames.TournamentGenerator.Type;


public sealed record LocalMatchId {

    const int NoProgression = -1;

    public int Value { get; init; }

    private LocalMatchId(int value) => Value = value;

    public static LocalMatchId New(int value) => new(value);

    public static LocalMatchId NewNoProgression() => new(NoProgression);
}
