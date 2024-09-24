using System.Numerics;
using System.Text.Json.Serialization;
using CouchPartyGames.TournamentGenerator.Exceptions;

namespace CouchPartyGames.TournamentGenerator.Builder.Size;


// <summary>
// Size of the draw
// </summary>
public sealed record DrawSize
{
    [JsonConverter(typeof(JsonStringEnumConverter<Size>))]
    public enum Size
    {
        Size2 = 2,
        Size4 = 4,
        Size8 = 8,
        Size16 = 16,
        Size32 = 32,
        Size64 = 64,
        Size128 = 128
    };

    public Size Value { get; }

    [JsonConstructor]
    private DrawSize(Size value) => Value = value;

    public static DrawSize New(DrawSize.Size size) => new(size);

    public static DrawSize NewFromOpponents(int numOpponents)
    {
        var size = (Size)BitOperations.RoundUpToPowerOf2((uint)numOpponents);
        if (!Enum.IsDefined(typeof(Size), size))
        {
            throw new InvalidDrawSizeException($"Unable to handle draw of size: {size}");
        }
            //https://learn.microsoft.com/en-us/dotnet/api/system.enum.isdefined?view=net-8.0&redirectedfrom=MSDN#System_Enum_IsDefined_System_Type_System_Object_
        return new(size);
    }

    public int ToTotalRounds() => Value switch
    {
        Size.Size2 => 1,
        Size.Size4 => 2,
        Size.Size8 => 3,
        Size.Size16 => 4,
        Size.Size32 => 5,
        Size.Size64 => 6,
        Size.Size128 => 7,
        _ => throw new InvalidDrawSizeException($"Unable to handle draw of size: ${Value}")
    };
}
