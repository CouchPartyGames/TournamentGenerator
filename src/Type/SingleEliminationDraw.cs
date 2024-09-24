namespace CouchPartyGames.TournamentGenerator.Type;

using CouchPartyGames.TournamentGenerator.Position;
using CouchPartyGames.TournamentGenerator.Exceptions;


public sealed class SingleEliminationDraw<TOpponent> 
{
   private readonly FinalsType _finalsType;

   private readonly bool _play3rdPlace;
   
   public List<Match<TOpponent>> Matches { get { return _matches; } }
   private List<Match<TOpponent>> _matches = new();

   private List<MatchWithId> _matchIds;

   
   public SingleEliminationDraw(CreateMatchIds matchIds, FinalsType finalsType = FinalsType.OneOfOne, bool play3rdPlace = false)
   {
      _finalsType = finalsType;
      _matchIds = matchIds.MatchByIds;
      _play3rdPlace = play3rdPlace;
   }

   public void CreateMatchProgressions()
   {
      var totalRounds = _matchIds.Max(m => m.Round);
      for (int round = 1; round < totalRounds; round++)
      {
         var nextRound = round + 1;
         var curRoundMatches = _matchIds.Where(m => m.Round == round).OrderBy(x => x.LocalMatchId).ToList();
         var nextRoundMatches = _matchIds.Where(m => m.Round == nextRound).OrderBy(x => x.LocalMatchId).ToList();

            // Get Match Ids for the Current and Next Round (after current round)
         AddMatchesToRound(round, curRoundMatches, nextRoundMatches);


         /*if (round == totalRounds - 1 && _play3rdPlace) {
            AddThirdPlaceMatch(99, 99);
         }*/
      }

      
      if (!IsFinalRound(totalRounds)) {
         throw new InvalidFinalRoundException("Final Round doesnt contain a single match");
      }

      var finalMatch = _matchIds
         .Where(m => m.Round == totalRounds)
         .ToList();

      AddFinals(finalMatch[0].LocalMatchId, totalRounds);
   }

   void AddMatchesToRound(int round, List<MatchWithId> curMatchIds, List<MatchWithId> nextMatchIds)
   {
      int prevId = 0;
      var chunkPairs = curMatchIds.Chunk<MatchWithId>(2);

         // Number of Pairs in the Current should match the Next Round
      if (chunkPairs.ToList().Count != nextMatchIds.Count)
      {
         throw new MismatchProgressionChunkSize("Current round's chunk doesn't match the next round's number of matches");
      }
      
      foreach (var pair in chunkPairs)
      {
         var matchToProgressTo = nextMatchIds[prevId];
         foreach (var curRoundMatch in pair)
         {
            var progressMatchId = matchToProgressTo.LocalMatchId;
            var match = round == 1 ? 
               Match<TOpponent>.New(curRoundMatch, progressMatchId) : 
               Match<TOpponent>.New(curRoundMatch, progressMatchId);

            _matches.Add(match);
         }
         prevId++;
      }
   }

   // Add Final Match(es) to Championship Round
   void AddFinals(int matchId, int round)
   {
      switch (_finalsType)
      {
         case FinalsType.OneOfOne:
            _matches.Add(Match<TOpponent>.NewNoProgression(matchId, round));
            break;

         case FinalsType.TwoOfThree:
            _matches.Add(Match<TOpponent>.New(matchId, round, matchId + 1));
            _matches.Add(Match<TOpponent>.New(matchId + 1, round + 1, matchId + 2));
            _matches.Add(Match<TOpponent>.NewNoProgression(matchId + 2, round + 2));
            break;

         case FinalsType.ThreeOfFive:
            _matches.Add(Match<TOpponent>.New(matchId, round, matchId + 1));
            _matches.Add(Match<TOpponent>.New(matchId + 1, round + 1, matchId + 2));
            _matches.Add(Match<TOpponent>.New(matchId + 2, round + 2, matchId + 3));
            _matches.Add(Match<TOpponent>.New(matchId + 3, round + 3, matchId + 4));
            _matches.Add(Match<TOpponent>.NewNoProgression(matchId + 4, round + 4));
            break;
      }
   }

   void AddThirdPlaceMatch(int matchId, int round) {
      _matches.Add(Match<TOpponent>.NewNoProgression(matchId, round));
   }

   bool IsFinalRound(int round) => _matchIds
      .Where(m => m.Round == round)
      .Count() == 1;
}
