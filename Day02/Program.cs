using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;

(int id, bool valid) parseLineValid(string line)
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

int parseLinePower(string line)
{
    int toReturn = 0;
    if (!string.IsNullOrEmpty(line))
    {
        string[] split = line.Split(':');
        if (split.Length > 1)
        {
            toReturn = getLinePower(split[1]);
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

IDictionary<string, int> getColorCounts(string subset)
{
    IDictionary<string, int> toReturn = new Dictionary<string, int>()
    {
        { "red", 0 },
        { "green", 0 },
        { "blue", 0 }
    };

    void setColorCount(string input, string color)
    {
        if (input.Contains(color))
        {
            toReturn[color] = int.Parse(input.Replace(color, "").Trim());
        }
    }


    foreach (string colorCubes in subset.Split(","))
    {
        setColorCount(colorCubes, "red");
        setColorCount(colorCubes, "green");
        setColorCount(colorCubes, "blue");
    }

    return toReturn;
}

bool isSubsetValid(string subset)
{
    IDictionary<string, int> colorCount = getColorCounts(subset);

    return colorCount["red"] <= 12
        && colorCount["green"] <= 13
        && colorCount["blue"] <= 14;
}

int getLinePower(string lineSuffix)
{
    IDictionary<string, int> lineColorMaxValues = new Dictionary<string, int>()
    {
        { "red", 0 },
        { "green", 0 },
        { "blue", 0 }
    };

    void setMaxValue(string color, IDictionary<string, int> colorCounts)
    {
        if (colorCounts[color] > lineColorMaxValues[color])
        {
            lineColorMaxValues[color] = colorCounts[color];
        }
    }

    foreach(string subset in lineSuffix.Trim().Split(";"))
    {
        IDictionary<string, int> colorCounts = getColorCounts(subset);
        setMaxValue("red", colorCounts);
        setMaxValue("green", colorCounts);
        setMaxValue("blue", colorCounts);
    }

    return lineColorMaxValues["red"] * lineColorMaxValues["green"] * lineColorMaxValues["blue"];
}

int sumValid = File.ReadAllLines("input.txt")
    .Where(l => !string.IsNullOrEmpty(l))
    .Select(l => parseLineValid(l))
    .Where(x => x.valid)
    .Select(x => x.id)
    .Sum();

int sumPowers = File.ReadAllLines("input.txt")
    .Where(l => !string.IsNullOrEmpty(l))
    .Select(l => parseLinePower(l))
    .Sum();

Console.WriteLine($"Part 1: {sumValid}");
Console.WriteLine($"Part 2: {sumPowers}");
Console.ReadLine();