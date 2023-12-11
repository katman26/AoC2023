using System.Drawing;

IEnumerable<string> fileLines = File.ReadAllLines("input.txt").ToList();

// Part 1
long maxCol = 0;
long maxRow = 0;
IEnumerable<LongPoint> galaxies = fileLines.SelectMany((l, rowIdx) =>
{
    if(maxCol == 0)
    {
        maxCol = l.Length - 1;
    }
    maxRow = rowIdx;
    return l.Select((c, idx) => c == '#' ? new LongPoint(idx, rowIdx) : null as LongPoint?)
        .Where(x => x.HasValue)
        .Select(x => x.Value);
}).ToList();

// Expand universe
IEnumerable<long> generateRange(long start, long count)
{
    IList<long> toReturn = new List<long>();
    for(long i = 0; i < count; i++)
    {
        toReturn.Add(start + i);
    }
    return toReturn;
}

bool part1 = false;
long factor = part1 ? 1 : 1000000;
IEnumerable<long> emptyCols = generateRange(0, maxCol + 1).Where(x => !galaxies.Select(p => p.X).Contains(x)).ToList();
IEnumerable<long> emptyRows = generateRange(0, maxRow + 1).Where(y => !galaxies.Select(p => p.Y).Contains(y)).ToList();
galaxies = galaxies.Select(g => new LongPoint(
    g.X + (emptyCols.Count(c => c < g.X) * (factor - (factor > 1 ? 1 : 0))), 
    g.Y + (emptyRows.Count(r => r < g.Y) * (factor - (factor > 1 ? 1 : 0)))
)).ToList();

// Make pairs
// https://stackoverflow.com/questions/5239635/how-take-each-two-items-from-ienumerable-as-a-pair
IEnumerable<Tuple<LongPoint, LongPoint>> pairs = galaxies.SelectMany((g, idx) =>
{
    return Enumerable.Range(idx + 1, galaxies.Count() - idx - 1).Select(i => new Tuple<LongPoint, LongPoint>(g, galaxies.ElementAt(i)));
}).ToList();

Console.WriteLine($"Sum: {pairs.Select(p => Math.Abs(p.Item2.X - p.Item1.X) + Math.Abs(p.Item2.Y - p.Item1.Y)).Sum()}");
Console.ReadLine();

public struct LongPoint : IEquatable<LongPoint>
{
    public LongPoint(long x, long Y)
    {
        this.X = x;
        this.Y = Y;
    }
    public long X { get; private set; } = 0;
    public long Y { get; private set; } = 0;

    public bool Equals(LongPoint other) => this.X == other.X && this.Y == other.Y;
}