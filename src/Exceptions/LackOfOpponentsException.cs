namespace CouchPartyGames.TournamentGenerator.Exceptions;

public sealed class LackOfOpponentsException(string message) : Exception(message);
