namespace CouchPartyGames.TournamentGenerator.Opponent;

using CouchPartyGames.TournamentGenerator.Exceptions;

// <summary>
// Random ordering of Opponents (Blind Draw Seeding)
// </summary>
public sealed class RandomOrder(List<IOpponent> opponents) : Order
{
    // <summary>
    // Dictionary of ordered opponents
    // </summary>
    public override Dictionary<int, IOpponent> Opponents
    {
        get
        {
            if (opponents.Count < MinParticipants)
            {
                throw new LackOfOpponentsException("Not enough participants");
            }

            Random rng = new Random();
            var orderedOpps = new Dictionary<int, IOpponent>();

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
