namespace CouchPartyGames.TournamentGenerator.Exceptions;

public sealed class MinimumOpponentsNotMetException(string message) : Exception(message);
