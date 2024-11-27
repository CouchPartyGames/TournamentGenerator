namespace CouchPartyGames.TournamentGenerator;

using CouchPartyGames.TournamentGenerator.Opponent;
using CouchPartyGames.TournamentGenerator.Position;
using CouchPartyGames.TournamentGenerator.Type;


public sealed class SingleEliminationBuilder<TOpponent> 
    where TOpponent : IOpponent 
{

    private readonly string _name;

    private List<TOpponent> _opponents = [];

    private TOpponent _byeOpponent;

    private IOpponentStartPosition _startingPositions;

    private TournamentSize _size = TournamentSize.NotSet;

    private TournamentSeeding _seeding = TournamentSeeding.Ranked;

    private Tournament3rdPlace _thirdPlace = Tournament3rdPlace.NoThirdPlace;

    private TournamentFinals _finals = TournamentFinals.OneOfOne;

    private bool _shouldHaveOpponents = false;

    public SingleEliminationBuilder(string name) {
        _name = name;
    }


    public SingleEliminationBuilder<TOpponent> WithOpponents(List<TOpponent> opponents, TOpponent byeOpponent)
    {
        _shouldHaveOpponents = true;
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
        Dictionary<int, TOpponent> opponents = new();

            // Create Starting Positions 
        _startingPositions = new DefaultStartingPositions(drawSize);


        if (_shouldHaveOpponents)
        {
            var order = Order<TOpponent>.Create(_seeding, _opponents);
            opponents = order.Opponents;
        }

            // Create Tournament with Progressions
        var singleElim = new SingleProgression(_startingPositions,
            _finals,
            _thirdPlace);
        var matches = singleElim
            .Matches
            .Select( x =>
            {
                if (_shouldHaveOpponents)
                {
                    var opp1 = opponents.ContainsKey(x.Position1) ? opponents[x.Position1] : _byeOpponent;
                    var opp2 = opponents.ContainsKey(x.Position2) ? opponents[x.Position2] : _byeOpponent;
                    return Match<TOpponent>.New(x, opp1, opp2);
                }
                    
                return Match<TOpponent>.New(x);
            })
            .ToList();


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
