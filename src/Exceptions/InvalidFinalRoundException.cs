namespace CouchPartyGames.TournamentGenerator.Exceptions;

public sealed class InvalidFinalRoundException(string message) : Exception(message);

public sealed class InvalidFirstRoundMatchesException(string message) : Exception(message);