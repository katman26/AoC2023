using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

int convertValue(string? input) => int.TryParse(input, out int val)
    ? val
    : input switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => 0
    };

int getSum(string filename, bool partTwo = false)
{
    int toReturn = 0;
    string regExPattern = partTwo ? "(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9)" : "(1|2|3|4|5|6|7|8|9)";

    foreach (string line in File.ReadAllLines(filename).Where(l => !string.IsNullOrEmpty(l)))
    {
        int first = convertValue(Regex.Match(line, regExPattern)?.Value);
        int last = convertValue(Regex.Match(line, regExPattern, RegexOptions.RightToLeft)?.Value);
        toReturn += int.Parse($"{first}{last}");
    }
    return toReturn;
}


Console.WriteLine($"Part 1: {getSum("input.txt")}");
Console.WriteLine($"Part 2: {getSum("input.txt", true)}");
Console.ReadKey();