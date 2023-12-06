long getWinningRounds(long time, long distance)
{
    long start = time / 2;
    long toReturn = 0;
    while(start * (time - start) > distance)
    {
        start--;
        toReturn++;
    }

    if (time % 2 == 0)
    {
        toReturn = toReturn + (toReturn - 1);
    }
    else
    {
        toReturn = toReturn * 2;
    }
    return toReturn;
}

long[] convertToArray(string numLine) => string.IsNullOrEmpty(numLine)
    ? new long[0]
    : numLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n.Trim())).ToArray();

long[] times = new long[0];
long[] distances = new long[0];
foreach(string line in File.ReadAllLines("input.txt"))
{
    if(line.Contains("Time:"))
    {
        times = convertToArray(line.Replace("Time:", "").Trim());
    }
    else if (line.Contains("Distance:"))
    {
        distances = convertToArray(line.Replace("Distance:", "").Trim());
    }
}

long part1product = 1;
for(long idx = 0; idx < times.Length; idx++)
{
    part1product = part1product * getWinningRounds(times[idx], distances[idx]);
}

long time = 0;
long distance = 0;
foreach (string line in File.ReadAllLines("input.txt"))
{
    if (line.Contains("Time:"))
    {
        time = long.Parse(line.Replace("Time:", "").Replace(" ", "").Trim());
    }
    else if (line.Contains("Distance:"))
    {
        distance = long.Parse(line.Replace("Distance:", "").Replace(" ", "").Trim());
    }
}


Console.WriteLine($"Part 1: {part1product}");
Console.WriteLine($"Part 2: {getWinningRounds(time, distance)}");
Console.ReadLine();