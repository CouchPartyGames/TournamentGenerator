namespace CouchPartyGames.TournamentGenerator.Exceptions;

public sealed class InvalidFirstRoundMatchesException(string message) : Exception(message);