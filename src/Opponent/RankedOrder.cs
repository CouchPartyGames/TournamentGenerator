namespace CouchPartyGames.TournamentGenerator.Opponent;

using CouchPartyGames.TournamentGenerator;
using CouchPartyGames.TournamentGenerator.Exceptions;

// <summary>
// Ranked Order of Opponents
// </summary>
public sealed class RankedOrder<TOpponent>(List<TOpponent> opponents) : Order<TOpponent> where TOpponent : IOpponent
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
                throw new LackOfOpponentsException("Not enough participants");
            }

            int i = _startIndex;
            var orderedOpponents = new Dictionary<int, TOpponent>();
            foreach (var opp in opponents.OrderByDescending(o => o.Rank))
            {
                orderedOpponents.Add(i, opp);
                i++;
            }

            return orderedOpponents;
        }
    }
}
