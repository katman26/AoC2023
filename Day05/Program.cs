using Day05;

IEnumerable<long> seeds = new List<long>();
IList<Map> maps = new List<Map>();
int mapIdx = 0;
Map currentMap = new Map(0, string.Empty);
IEnumerable<long> mapSeeds(string line)
{
    IEnumerable<long> toReturn = new List<long>();
    if (!string.IsNullOrEmpty(line))
    {
        toReturn = line.Replace("seeds:", "").Trim().Split(" ").Select(s => long.Parse(s.Trim()));
    }

    return toReturn;
}

foreach (string line in File.ReadAllLines("input.txt"))
{
    if (!string.IsNullOrEmpty(line))
    {
        if (line.Contains("seeds:"))
        {
            seeds = mapSeeds(line);
        }
        else if (line.Contains("map:"))
        {
            mapIdx++;
            currentMap = new Map(mapIdx, line.Replace("map:", "").Trim());
            maps.Add(currentMap);
        }
        else
        {
            IEnumerable<long> values = line.Split(" ").Select(v => long.Parse(v.Trim()));
            if (values.Count() > 2)
            {
                currentMap.FillSourceToDesination(values.Skip(1).First(), values.First(), values.Skip(2).First());
            }
        }
    }
}

long getLocation(long seed)
{
    long toReturn = seed;
    foreach (Map m in maps)
    {
        toReturn = m.GetDestination(toReturn);
    }
    return toReturn;
}

long? part1Location = null;
foreach (long s in seeds)
{
    long newLocation = getLocation(s);
    if (!part1Location.HasValue || newLocation < part1Location)
    {
        part1Location = newLocation;
    }
}

// Part 2 - takes about three hours to run
long? part2Location = null;
IList<long> part2Seeds = new List<long>();
Parallel.For(0, seeds.Count() / 2, seedIdx =>
{
    long start = seeds.Skip(2 * seedIdx).First();
    long range = seeds.Skip((2 * seedIdx) + 1).First();
    Parallel.For(0, range, index =>
    {
        long newLocation = getLocation(start + index);
        if (!part2Location.HasValue || newLocation < part2Location)
        {
            part2Location = newLocation;
        }
    });
});

Console.WriteLine($"Part 1: {part1Location}");
Console.WriteLine($"Part 2: {part2Location}");
Console.ReadLine();