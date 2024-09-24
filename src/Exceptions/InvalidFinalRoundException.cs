namespace CouchPartyGames.TournamentGenerator.Exceptions;

public sealed class InvalidFinalRoundException(string message) : Exception(message);
