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
galaxies = galaxies.Select(g => new LongPoint(g.X + (emptyCols.Count(c => c < g.X) * factor) , g.Y + (emptyRows.Count(r => r < g.Y)) * factor)).ToList();

// Make pairs
// https://stackoverflow.com/questions/5239635/how-take-each-two-items-from-ienumerable-as-a-pair
IEnumerable<Tuple<LongPoint, LongPoint>> pairs = galaxies.SelectMany((g, idx) =>
{
    return Enumerable.Range(idx + 1, galaxies.Count() - idx - 1).Select(i => new Tuple<LongPoint, LongPoint>(g, galaxies.ElementAt(i)));
}).ToList();

// Ended up using https://topaz.github.io/paste/#XQAAAQAzAQAAAAAAAAA8HMGAo4hPR976iAhi0wrjlImYJpx8+9ayzfqCdblu6ds63dTpsVDs+WZcg26ZQQNHhdtmKSbwBDoGIYV3dGYocTfKbxjKhDo5Otm9PpOvs5j+vVG2FlTJkibnb0I1g3m1Oisd4o5DfrPk4CoFrXHeTz6vds04K5wL1qOrtW5ttHHijcNU9B1kh8jo3jlD5a1OvrCpHhOcS1GR19QY8tJ7O1UmLsnNq4OTG3Y2mTLH6DJnTBc/Xe7gkPBqioSMTM12ArDuCW1Cgstt+/94CBUA
// I still don't think that the output is right.  Running it for 10 on the original map, I get 1112 and not 1030.
// I've even exported the pairs, checked the added distance, and then calcualted the sum.  I get 1112 every time which matches my algorithm, but not the answer

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