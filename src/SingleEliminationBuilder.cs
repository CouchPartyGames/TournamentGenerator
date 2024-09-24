namespace CouchPartyGames.TournamentGenerator;

using CouchPartyGames.TournamentGenerator.Builder.Size;
using CouchPartyGames.TournamentGenerator.Builder.Opponent;
using CouchPartyGames.TournamentGenerator.Builder.Position;
using CouchPartyGames.TournamentGenerator.Builder.Type;


public sealed class SingleEliminationBuilder<TOpponent> 
    where TOpponent : IOpponent {

    private DrawSize _drawSize;
    private string _name;

    private List<TOpponent> _opponents = new();

    private IOpponentStartPosition _startingPositions;

    private bool _play3rdPlace = false;

    private FinalsType _finalsType = FinalsType.OneOfOne;

    public SingleEliminationBuilder(string name) {
        _name = name;
    }

    public SingleEliminationBuilder<TOpponent> SetFinalsType(FinalsType finalsType) {
        _finalsType = finalsType;
        return this;
    }

    public SingleEliminationBuilder<TOpponent> EnableThirdPlace(bool enableThirdPlace) {
        _play3rdPlace = enableThirdPlace;
        return this;
    }

    public SingleEliminationBuilder<TOpponent> WithOpponents(List<TOpponent> opponents) {
        _opponents = opponents;
        return this;
    }

    public SingleEliminationBuilder<TOpponent> SetSeeding() {
        return this;
    }

    public Tournament<TOpponent> Build() {
        if (_opponents.Count == 0) {
            _drawSize = DrawSize.New(DrawSize.Size.Size16);
        } else {
            _drawSize = DrawSize.NewFromOpponents(_opponents.Count);
        }
            // Create Starting Positions 
        _startingPositions = new DefaultStartingPositions(_drawSize);

            // Create Ids for each Match
        var matchIds = new CreateMatchIds(_startingPositions);

            // Create Tournament
        var single = new SingleEliminationDraw<TOpponent>(matchIds, _finalsType);
        single.CreateMatchProgressions();
        var matches = single.Matches;

        return new Tournament<TOpponent> {
            Name = _name,
            AllowThirdPlace = _play3rdPlace,
            FinalsType = _finalsType,
            ActiveOpponents = _opponents,
            Matches = matches
        };
    }
}
