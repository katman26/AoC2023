using Day03;
using System.Drawing;
using System.Text.RegularExpressions;

Regex partNumbersRegEx = new Regex(@"\d+", RegexOptions.Compiled);
Regex symbolRegEx = new Regex("([^\\.|\\d])", RegexOptions.Compiled);
Regex starRegEx = new Regex(@"\*", RegexOptions.Compiled);
List<PartNumber> partNumbers = new List<PartNumber>();
List<Point> symbols = new List<Point>();
List<Point> stars = new List<Point>();

int rowIdx = 1;
foreach(string line in File.ReadAllLines("input.txt"))
{
    partNumbers.AddRange(partNumbersRegEx.Matches(line).Select(m => new PartNumber(int.Parse(m.Value), rowIdx, m.Index + 1, m.Index + m.Value.Length)));
    symbols.AddRange(symbolRegEx.Matches(line).Select(m => new Point(m.Index + 1, rowIdx)));
    stars.AddRange(starRegEx.Matches(line).Select(m => new Point(m.Index + 1, rowIdx)));
    rowIdx++;
}

int sumPart1 = symbols.Select(p => partNumbers.Where(pn => pn.IsAdjacent(p)).Sum(pn => pn.Value)).Sum();
int sumPart2 = 0;
foreach(Point p in stars)
{
    IEnumerable<PartNumber> adjacentPartNumbers = partNumbers.Where(pn => pn.IsAdjacent(p));
    if(adjacentPartNumbers.Count() == 2)
    {
        sumPart2 += (adjacentPartNumbers.First().Value * adjacentPartNumbers.Last().Value);
    }
}

Console.WriteLine($"Part 1: {sumPart1}");
Console.WriteLine($"Part 2: {sumPart2}");

Console.ReadKey();