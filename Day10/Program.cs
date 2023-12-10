using System.Drawing;
using System.Text.RegularExpressions;

IEnumerable<string> map = File.ReadAllLines("input.txt");
char? getChar(Point point) => point.X >= 0 && point.Y >= 0
    ? map.ElementAt(point.Y)[point.X]
    : null;

Point getStartingPoint() => map.Select((line, idx) => (line, idx))
    .Where(x => x.line.Contains("S"))
    .Select(x => new Point(x.line.IndexOf("S"), x.idx))
    .First();

Point getDirectionFromPoint(Point p, Direction direction) => direction switch
{
    Direction.North => new Point(p.X, p.Y - 1),
    Direction.South => new Point(p.X, p.Y + 1),
    Direction.East => new Point(p.X + 1, p.Y),
    Direction.West => new Point(p.X - 1, p.Y),
    _ => new Point(0,0)
};

(Direction d1, Direction d2) getPoints(char c) => c switch
{
    '|' => (Direction.North, Direction.South),
    '-' => (Direction.East, Direction.West),
    'L' => (Direction.North, Direction.East),
    'J' => (Direction.North, Direction.West),
    '7' => (Direction.South, Direction.West),
    'F' => (Direction.South, Direction.East),
    _ => (Direction.Unknown, Direction.Unknown)
};

Point getNextPoint(Point currentPosition, char c, Direction sourceDirection)
{
    (Direction d1, Direction d2) charPoint = getPoints(c);
    Direction nextDirection = sourceDirection == charPoint.d1
        ? charPoint.d2
        : charPoint.d1;

    return getDirectionFromPoint(currentPosition, nextDirection);
}

bool canMove(Point currentPoint, Direction direction, Direction sourceDirection)
{
    bool toReturn = false;
    char? dir = getChar(getDirectionFromPoint(currentPoint, direction));
    if (dir!= null)
    {
        (Direction d1, Direction d2) point = getPoints(dir.Value);
        toReturn = point.d1 == sourceDirection || point.d2 == sourceDirection;
    }
    return toReturn;
}

Direction getOppositeDirection(Direction direction) => direction switch
{
    Direction.North => Direction.South,
    Direction.South => Direction.North,
    Direction.East => Direction.West,
    Direction.West => Direction.East,
    _ => Direction.Unknown
};

Point startingPoint = getStartingPoint();
IDictionary<Direction, bool> canMoveDirection = new Dictionary<Direction, bool>()
{
    { Direction.North, canMove(startingPoint, Direction.North, Direction.South) },
    { Direction.South, canMove(startingPoint, Direction.South, Direction.North) },
    { Direction.West, canMove(startingPoint, Direction.West, Direction.East) },
    { Direction.East, canMove(startingPoint, Direction.East, Direction.West) }
};

Direction currentDirection = canMoveDirection.FirstOrDefault(kv => kv.Value).Key;
Point currentPoint = getDirectionFromPoint(startingPoint, canMoveDirection.FirstOrDefault(kv => kv.Value).Key);
IList<Point> path = new List<Point>();
do
{
    path.Add(currentPoint);
    char c = getChar(currentPoint).Value;
    (Direction d1, Direction d2) directions = getPoints(c);
    Direction nextDirection = directions.d1 == getOppositeDirection(currentDirection)
        ? directions.d2
        : directions.d1;
    currentPoint = getDirectionFromPoint(currentPoint, nextDirection);
    currentDirection = nextDirection;
} while (currentPoint != startingPoint);

long part1Path = path.Count() / 2;
if(part1Path % 2 == 1)
{
    part1Path++;
}
// Pulled from https://github.com/Bpendragon/AdventOfCodeCSharp/blob/fa93dc4a/AdventOfCode/Solutions/Year2023/Day10-Solution.cs
// Came close, but not quite right.  Ended up using https://github.com/zivnadel/advent-of-code/blob/master/2023/python/day10.py
IEnumerable<string> cleanedMap = map.Select((l, y) =>
{
    string output = string.Join("", l.Select((c, x) => path.Contains(new Point(x, y))
        ? '|'
        : '.'));

    return Regex.Replace(Regex.Replace(output, "F-*7|L-*J", string.Empty), "F-*J|L-*7", "|");
});

int part2Ans = 0;

foreach (string line in cleanedMap)
{
    int parity = 0;
    foreach (char c in line)
    {
        if (c == '|')
        {
            parity++;
        }
        if (c == '.' && parity % 2 == 1)
        {
            part2Ans++;
        }
    }
}

Console.WriteLine($"Part 1: {part1Path}");
Console.WriteLine($"Part 2: {part2Ans}");
Console.ReadLine();

enum Direction
{
    North,
    South,
    East,
    West,
    Unknown
}