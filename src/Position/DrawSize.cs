using System.Numerics;
using System.Text.Json.Serialization;
using CouchPartyGames.TournamentGenerator.Exceptions;

namespace CouchPartyGames.TournamentGenerator.Position;


// <summary>
// Size of the draw
// </summary>
public sealed record DrawSize
{
    //[JsonConverter(typeof(JsonStringEnumConverter<Size>))]
    public TournamentSize Value { get; }

    //[JsonConstructor]
    private DrawSize(TournamentSize value) => Value = value;

    public static DrawSize New(TournamentSize size) => new(size);

    public static DrawSize NewRoundBase2(int numOpponents)
    {
        var size = (TournamentSize)BitOperations.RoundUpToPowerOf2((uint)numOpponents);
        if (!Enum.IsDefined(typeof(TournamentSize), size))
        {
            throw new InvalidDrawSizeException($"Invalid draw size: {size}");
        }
        return new(size);
    }

    public int ToTotalRounds() => Value switch
    {
        TournamentSize.Size2 => 1,
        TournamentSize.Size4 => 2,
        TournamentSize.Size8 => 3,
        TournamentSize.Size16 => 4,
        TournamentSize.Size32 => 5,
        TournamentSize.Size64 => 6,
        TournamentSize.Size128 => 7,
        _ => throw new InvalidDrawSizeException($"Invalid draw size: ${Value}")
    };
}