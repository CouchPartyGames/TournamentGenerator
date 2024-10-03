namespace CouchPartyGames.TournamentGenerator.Type;

using CouchPartyGames.TournamentGenerator.Position;

public sealed record MatchProgression
{
    public int Round { get; private set; }
    public int LocalMatchId { get; private set; }
    public int Position1 { get; private set; } = -1; 
    public int Position2 { get; private set; } = -1;

    public int WinProgressionMatchId { get; private set; }

    public int LoseProgressionMatchId { get; private set; }

    private MatchProgression(int round, int localMatchId, int position, int position2 = -1) {
        Round = round;
        LocalMatchId = localMatchId;
        Position1 = position;
        Position2 = position2;
    }

    public static MatchProgression CreateOtherRounds(int round, int matchId) =>
        new(round, matchId, -1, -1);


    public static MatchProgression Create1stRound(int round, int matchId, OpponentStartPosition match) =>
        new(round, matchId, match.FirstOpponentPosition, match.SecondOpponentPosition);

    public void UpdateWinProgression(int localMatchId) {
        WinProgressionMatchId = localMatchId;
    }

    public void UpdateLoseProgression(int localMatchId) {
        LoseProgressionMatchId = localMatchId;
    }
}
