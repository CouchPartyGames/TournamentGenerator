# TournamentGenerator
Simple Tournament Generator 




## Features

- Single elimination tournament 
- Double elimination tournament


## Installation

Add `TournamentGenerator` to your existing project
```
dotnet add package CouchPartyGames.TournamentGenerator
```


## Basic Usage

Create a new console application using .NET 8
```
dotnet new console -o TournamentExample
cd TournamentExample
dotnet add package CouchPartyGames.TournamentGenerator
```


Simple Example that creates a tournament
```
using CouchPartyGames.TournamentGenerator;

	// Create a List of Opponents
List<MyOpponent> opponents = [
    new("Pete", 3),
    new("Steve", 3),
    new("Bob", 8),
    new("Ric", 5),
    new("Andre", 2),
    new("Bill", 1),
    new("Tim", 9),
	new("Jim", 22),
	new("John", 33)
];

	// Create a bye opponent
var byeOpponent = new MyOpponent("Bye", int.MaxValue);

var tournament = new SingleEliminationBuilder<MyOpponent>("Bob's Tournament")
    .WithOpponents(opponents, byeOpponent)
    .SetSize(TournamentSize.Size16)
    .SetSeeding(TournamentSeeding.Ranked)
    .SetFinals(TournamentFinals.TwoOfThree)
    .Set3rdPlace(Tournament3rdPlace.ThirdPlace)
    .Build();

foreach(var match in tournament.Matches) {
    Console.WriteLine(match);
}

    // Create Opponent object
    // *Note: Must extend IOpponent interface
public record MyOpponent(string Name, int Rank) : IOpponent;
```
