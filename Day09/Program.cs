long calculateNextValue(List<long> numbers)
{
    List<List<long>> numberList = new List<List<long>>() { numbers };
    List<long> currentList = numbers;
    while(!currentList.All(n => n == 0))
    {
        List<long> newRow = new List<long>();
        for(int idx = 0; idx < currentList.Count() - 1; idx++)
        {
            //newRow.Add(Math.Abs(currentList.ElementAt(idx) - currentList.ElementAt(idx + 1)));
            newRow.Add(currentList.ElementAt(idx + 1) - currentList.ElementAt(idx));
        }
        currentList = newRow;
        numberList.Add(currentList);
    }


    if(!currentList.Any())
    {
        currentList.Add(0);
        numberList.Add(currentList);
    }

    for (int idx = numberList.Count() - 2; idx >= 0; idx--)
    {
        currentList = numberList.ElementAt(idx);
        currentList.Add(currentList.Last() + numberList.ElementAt(idx + 1).Last());
    }

    return numberList.First().Last();
}

long calculateFirstValue(List<long> numbers)
{
    List<List<long>> numberList = new List<List<long>>() { numbers };
    List<long> currentList = numbers;
    while (!currentList.All(n => n == 0))
    {
        List<long> newRow = new List<long>();
        for (int idx = 0; idx < currentList.Count() - 1; idx++)
        {
            //newRow.Add(Math.Abs(currentList.ElementAt(idx) - currentList.ElementAt(idx + 1)));
            newRow.Add(currentList.ElementAt(idx) - currentList.ElementAt(idx + 1));
        }
        currentList = newRow;
        numberList.Add(currentList);
    }


    if (!currentList.Any())
    {
        currentList.Add(0);
        numberList.Add(currentList);
    }

    for (int idx = numberList.Count() - 2; idx >= 0; idx--)
    {
        currentList = numberList.ElementAt(idx);
        currentList.Insert(0, currentList.First() + numberList.ElementAt(idx + 1).First());
    }

    return numberList.First().First();
}


long part1Sum = 0;
foreach(string line in File.ReadAllLines("input.txt"))
{
    part1Sum += calculateNextValue(line.Split(' ').Select(n => long.Parse(n.Trim())).ToList());
}


long part2Sum = 0;
foreach (string line in File.ReadAllLines("input.txt"))
{
    part2Sum += calculateFirstValue(line.Split(' ').Select(n => long.Parse(n.Trim())).ToList());
}

Console.WriteLine($"Part 1: {part1Sum}");
Console.WriteLine($"Part 2: {part2Sum}");
Console.ReadLine();