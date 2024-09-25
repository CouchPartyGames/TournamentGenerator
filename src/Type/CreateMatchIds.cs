namespace CouchPartyGames.TournamentGenerator.Type;

using CouchPartyGames.TournamentGenerator.Position;


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
