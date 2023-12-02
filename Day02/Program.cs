using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;

(int id, bool valid) parseLine(string line)
{
    (int id, bool valid) toReturn = (id: 0, valid: false);
    if(!string.IsNullOrEmpty(line))
    {
        string[] split = line.Split(':');
        if(split.Length > 1)
        {
            toReturn.id = getGameId(split[0]);
            toReturn.valid = isGameValid(split[1]);
        }
    }
    return toReturn;
}

int getGameId(string linePrefix)
{
    int toReturn = 0;
    if(!string.IsNullOrEmpty(linePrefix))
    {
        toReturn = int.Parse(linePrefix.Replace("Game ", "").Trim());
    }
    return toReturn;
}

bool isGameValid(string lineSuffix) => lineSuffix.Trim().Split(";").All(s => isSubsetValid(s.Trim()));
bool isSubsetValid(string subset)
{
    IDictionary<string, int> colorCount = new Dictionary<string, int>()
    {
        { "red", 0 },
        { "green", 0 },
        { "blue", 0 }
    };

    void setColorCount(string input, string color)
    {
        if (input.Contains(color))
        {
            colorCount[color] = int.Parse(input.Replace(color, "").Trim());
        }
    }


    foreach (string colorCubes in subset.Split(","))
    {
        setColorCount(colorCubes, "red");
        setColorCount(colorCubes, "green");
        setColorCount(colorCubes, "blue");
    }

    return colorCount["red"] <= 12
        && colorCount["green"] <= 13
        && colorCount["blue"] <= 14;
}

int sum = File.ReadAllLines("input.txt")
    .Where(l => !string.IsNullOrEmpty(l))
    .Select(l => parseLine(l))
    .Where(x => x.valid)
    .Select(x => x.id)
    .Sum();

Console.WriteLine($"Part 1: {sum}");
Console.ReadLine();