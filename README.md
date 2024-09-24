# TournamentGenerator
Simple Tournament Generator 




## Features

- 
- 


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
using CouchPartyGames.TournamentGen;

List<MyOpponent> opponents = [
    new("Pete", 3),
    new("Steve", 3),
    new("Bob", 8),
    new("Ric", 5),
    new("Andre", 2),
    new("Bill", 1),
    new("Tim", 1)
];

var tournament = new SingleEliminationBuilder<MyOpponent>("Jete's Tournament")
    .WithOpponents(opponents)
    .SetFinalsType(FinalsType.TwoOfThree)
    .Build();

foreach(var match in tournament.Matches) {
    Console.WriteLine(match);
}

	// Create Opponent
public record MyOpponent(string Name, int Rank) : IOpponent;
```
