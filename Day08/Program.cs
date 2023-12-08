string directions = "";
IDictionary<string, Dictionary<char, string>> map = new Dictionary<string, Dictionary<char, string>>();

int lineIdx = 0;
foreach(string line in File.ReadAllLines("input.txt"))
{
    if(lineIdx == 0)
    {
        directions = line.Trim().ToUpper();
    }
    else if(lineIdx > 1)
    {
        string[] lineSplit = line.Split('=');
        if(lineSplit.Length > 1)
        {
            string[] mapSplit = lineSplit[1].Replace("(", "").Replace(")", "").Split(',');
            if(mapSplit.Length > 1)
            {
                map[lineSplit[0].Trim().ToUpper()] = new Dictionary<char, string>()
                {
                    { 'L', mapSplit[0].Trim().ToUpper() },
                    { 'R', mapSplit[1].Trim().ToUpper() }
                };
            }
        }
    }

    lineIdx++;
}

long getStepCount(string source, Func<string,bool> destinationCheck)
{
    long toReturn = 0;
    string current = source;
    while(!destinationCheck(current))
    {
        foreach (char dir in directions)
        {
            current = map[current][dir];
            toReturn++;

            if (destinationCheck(current))
            {
                break;
            }
        }
    }
    return toReturn;
}

// Brute force - didn't complete in three hours
//long part2StepCount = 0;
//IEnumerable<string> currentNodes = map.Keys.Where(k => k.EndsWith("A")).ToList();
//while(!currentNodes.All(n => n.EndsWith("Z")))
//{
//    foreach(char dir in directions)
//    {
//        currentNodes = currentNodes.Select(n => map[n][dir]).ToList();
//        part2StepCount++;
//        if(currentNodes.All(n => n.EndsWith("Z")))
//        {
//            break;
//        }
//    }
//}

long leastCommonMultiplier(params long[] numbers)
{
    long greatestCommonDenominator(long n1, long n2)
    {
        if (n2 == 0)
        {
            return n1;
        }
        else
        {
            return greatestCommonDenominator(n2, n1 % n2);
        }
    }
    return numbers.Aggregate((S, val) => S * val / greatestCommonDenominator(S, val));
}

IEnumerable<long> pathLengths = map.Keys.Where(k => k.EndsWith("A")).Select(k => getStepCount(k, s => s.EndsWith("Z")));

Console.WriteLine($"Part 1: {getStepCount("AAA", s => s == "ZZZ")}");
Console.WriteLine($"Part 2: {leastCommonMultiplier(pathLengths.ToArray())}");
Console.ReadLine();