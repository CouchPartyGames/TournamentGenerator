namespace CouchPartyGames.TournamentGenerator;

using CouchPartyGames.TournamentGenerator.Opponent;
using CouchPartyGames.TournamentGenerator.Position;
using CouchPartyGames.TournamentGenerator.Type;


public sealed class SingleEliminationBuilder<TOpponent> 
    where TOpponent : IOpponent 
{

    private string _name;

    private List<TOpponent> _opponents = new();

    private TOpponent _byeOpponent;

    private IOpponentStartPosition _startingPositions;

    private TournamentSize _size = TournamentSize.NotSet;

    private TournamentSeeding _seeding = TournamentSeeding.Ranked;

    private Tournament3rdPlace _thirdPlace = Tournament3rdPlace.NoThirdPlace;

    private TournamentFinals _finals = TournamentFinals.OneOfOne;

    public SingleEliminationBuilder(string name) {
        _name = name;
    }


    public SingleEliminationBuilder<TOpponent> WithOpponents(List<TOpponent> opponents, TOpponent byeOpponent) {
        _opponents = opponents;
        _byeOpponent = byeOpponent;
        return this;
    }

    public SingleEliminationBuilder<TOpponent> SetFinals(TournamentFinals finals) {
        _finals = finals;
        return this;
    }

    public SingleEliminationBuilder<TOpponent> SetSize(TournamentSize size) {
        _size = size;
        return this;
    }

    public SingleEliminationBuilder<TOpponent> SetSeeding(TournamentSeeding seeding) {
        _seeding = seeding;
        return this;
    }

    public SingleEliminationBuilder<TOpponent> Set3rdPlace(Tournament3rdPlace thirdPlace) {
        _thirdPlace = thirdPlace;
        return this;
    }


    public Tournament<TOpponent> Build() {
        var drawSize = GetDrawSize();

            // Create Starting Positions 
        _startingPositions = new DefaultStartingPositions(drawSize);

        var singleElim = new CreateMatchProgression(_startingPositions,
            _finals,
            _thirdPlace);
        var singleMatches = singleElim.Matches;


            // Create Ids for each Match
        var matchIds = new CreateMatchIds(_startingPositions);
        

        var order = Order<TOpponent>.Create(_seeding, _opponents);
        var opponents = order.Opponents;

            // Create Tournament with Progressions
        var draw = new SingleEliminationDraw<TOpponent>(matchIds, _finals);
        draw.CreateMatchProgressions();
        if (opponents.Count > 0) {
            draw.WithOpponents(opponents);
        }
        var matches = draw.Matches;

        return new Tournament<TOpponent> {
            Name = _name,
            Size = (int)_size,
            Seeding = nameof(_seeding),
            ThirdPlace = _thirdPlace == Tournament3rdPlace.ThirdPlace ? "Yes" : "No",
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
