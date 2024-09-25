namespace CouchPartyGames.TournamentGenerator.Opponent;

using CouchPartyGames.TournamentGenerator;
using CouchPartyGames.TournamentGenerator.Exceptions;

// <summary>
// Random ordering of Opponents (Blind Draw Seeding)
// </summary>
public sealed class RandomOrder<TOpponent>(List<TOpponent> opponents) : Order<TOpponent> 
    where TOpponent : IOpponent
{
    // <summary>
    // Dictionary of ordered opponents
    // </summary>
    public override Dictionary<int, TOpponent> Opponents
    {
        get
        {
            if (opponents.Count < MinParticipants)
            {
                throw new LackOfOpponentsException($"Found {opponents.Count} opponents! Tournament requires at least {MinParticipants}!");
            }

            Random rng = new Random();
            var orderedOpps = new Dictionary<int, TOpponent>();

            int i = _startIndex;
            foreach (var opp in opponents.OrderBy(a => rng.Next()).ToList() )
            {
                orderedOpps.Add(i, opp);
                i++;
            }

            return orderedOpps;
        }
    }
}
