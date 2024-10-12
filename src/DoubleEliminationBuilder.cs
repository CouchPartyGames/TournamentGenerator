namespace CouchPartyGames.TournamentGenerator;

using CouchPartyGames.TournamentGenerator.Opponent;
using CouchPartyGames.TournamentGenerator.Position;
using CouchPartyGames.TournamentGenerator.Type;

public sealed class DoubleEliminationBuilder<TOpponent> 
    where TOpponent : IOpponent 
{
    private string _name;

    private List<TOpponent> _opponents = new();

    private TOpponent _byeOpponent;

    private IOpponentStartPosition _startingPositions;

    private TournamentSize _size = TournamentSize.NotSet;

    private TournamentSeeding _seeding = TournamentSeeding.Ranked;


    private TournamentFinals _finals = TournamentFinals.OneOfOne;

    public DoubleEliminationBuilder(string name) {
        _name = name;
    }

    public DoubleEliminationBuilder<TOpponent> WithOpponents(List<TOpponent> opponents, TOpponent byeOpponent) {
        _opponents = opponents;
        _byeOpponent = byeOpponent;
        return this;
    }

    public DoubleEliminationBuilder<TOpponent> SetSize(TournamentSize size) {
        _size = size;
        return this;
    }

    public DoubleEliminationBuilder<TOpponent> SetSeeding(TournamentSeeding seeding) {
        _seeding = seeding;
        return this;
    }

    public Tournament<TOpponent> Build() {
        var drawSize = GetDrawSize();
        _startingPositions = new DefaultStartingPositions(drawSize);
        _size = drawSize.Value;

        var order = Order<TOpponent>.Create(_seeding, _opponents);
        var opponents = order.Opponents;

        var doubleElim = new DoubleProgression(_startingPositions);
        var matches = doubleElim.Matches
            .Select( x => Match<TOpponent>.New(x) )
            .ToList();

        return new Tournament<TOpponent> {
            Name = _name,
            Size = (int) _size,
            Seeding = nameof(_seeding),
            ThirdPlace = "No",
            FinalsType = _finals,
            ActiveOpponents = _opponents,
            Matches = matches
        };
    }

    private DrawSize GetDrawSize() {
        if (_size != TournamentSize.NotSet) {
            return DrawSize.New(_size);
        }
        return DrawSize.NewRoundBase2(_opponents.Count);
    }
}