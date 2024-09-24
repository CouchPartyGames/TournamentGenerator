namespace CouchPartyGames.TournamentGenerator.Builder.Type;

using CouchPartyGames.TournamentGenerator.Builder.Position;

public sealed record MatchWithId
{
    public int Round { get; private set; }
    public int LocalMatchId { get; private set; }
    public int Position1 { get; private set; } = -1; 
    public int Position2 { get; private set; } = -1;

    private MatchWithId(int round, int localMatchId, int position, int position2 = -1) {
        Round = round;
        LocalMatchId = localMatchId;
        Position1 = position;
        Position2 = position2;
    }

    public static MatchWithId CreateOtherRounds(int round, int matchId) =>
        new(round, matchId, -1, -1);


    public static MatchWithId Create1stRound(int round, int matchId, OpponentStartPosition match) =>
        new(round, matchId, match.FirstOpponentPosition, match.SecondOpponentPosition);
}


public sealed class CreateMatchIds
{

    private readonly IOpponentStartPosition _positions;

    public List<MatchWithId> MatchByIds { get; private set; } = new();

    public CreateMatchIds(IOpponentStartPosition positions)
    {
        _positions = positions;
        Create();
    }


    void Create()
    {
        var matchId = 1;
        for (int round = 1; round <= _positions.DrawSize.ToTotalRounds(); round++)
        {
            var totalMatches = GetTotalMatchesInRound(round);
            for (int j = 0; j < totalMatches; j++)
            {
                CreateMatchId(round, matchId);
                matchId++;
            }
        }
    }

    void CreateMatchId(int round, int matchId)
    {
        var match = round == 1 ? 
            MatchWithId.Create1stRound(round, matchId, _positions.Matches[matchId - 1]) :
            MatchWithId.CreateOtherRounds(round, matchId);

        MatchByIds.Add(match);
    }

    int GetTotalMatchesInRound(int round) => (int)_positions.DrawSize.Value / (int)Math.Pow(2, round);
}
