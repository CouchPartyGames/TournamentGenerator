namespace CouchPartyGames.TournamentGenerator.Opponent;

using CouchPartyGames.TournamentGenerator.Exceptions;

// <summary>
// Ranked Order of Opponents
// </summary>
public sealed class RankedOrder(List<IOpponent> opponents) : Order
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

            int i = _startIndex;
            var orderedOpponents = new Dictionary<int, IOpponent>();
            foreach (var opp in opponents.OrderByDescending(o => o.Rank))
            {
                orderedOpponents.Add(i, opp);
                i++;
            }

            return orderedOpponents;
        }
    }
}
