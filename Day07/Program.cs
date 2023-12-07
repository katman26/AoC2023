(string hand, int bid) parseLine(string line)
{
    (string hand, int bid) toReturn = (hand: string.Empty, bid: 0);
    string[] lineSplit = line.Split(' ');
    if(lineSplit.Length > 1)
    {
        toReturn.hand = lineSplit[0].Trim();
        toReturn.bid = int.Parse(lineSplit[1].Trim());
    }
    return toReturn;
}

IEnumerable<(string hand, int bid)> hands = File.ReadAllLines("input.txt")
    .Select(l => parseLine(l))
    .ToList();

long part1Sum = hands
    .OrderByDescending(x => x.hand, new HandComparer())
    .Select((x, i) => x.bid * (i + 1))
    .Sum();

long part2Sum = hands
    .OrderByDescending(x => x.hand, new HandComparer(true))
    .Select((x, i) => x.bid * (i + 1))
    .Sum();

Console.WriteLine($"Part 1: {part1Sum}");
Console.WriteLine($"Part 2: {part2Sum}");
Console.ReadLine();

class HandComparer : IComparer<string>
{
    public HandComparer(bool handleJoker = false)
    {
        this.IncludeJoker = handleJoker;
    }
    private bool IncludeJoker { get; } = false;
    private enum HandStrength
    {
        FiveOfAKind = 1,
        FourOfAKind = 2,
        FullHouse = 3,
        ThreeOfAKind = 4,
        TwoPair = 5,
        Pair = 6,
        None = 7
    }

    private string HandleJoker(string hand)
    {
        if(hand.Contains('J'))
        {
            IEnumerable<IGrouping<char, char>> groups = hand.Replace("J", "").GroupBy(c => c);
            if(groups.Count() > 1 )
            {
                char toReplace = groups.OrderByDescending(g => g.Count()).First().First();
                hand = hand.Replace('J', toReplace);
            }
            else
            {
                hand = "AAAAA";
            }
        }
        return hand;
    }

    private HandStrength GetHandStrength(string hand)
    {
        HandStrength toReturn = HandStrength.None;

        if(this.IncludeJoker)
        {
            hand = this.HandleJoker(hand);
        }

        IEnumerable<IGrouping<char, char>> groups = hand.GroupBy(c => c);
        if(groups.Count() == 1)
        {
            toReturn = HandStrength.FiveOfAKind;
        }
        else if(groups.Count() == 2)
        {
            if(groups.First().Count() == 1 || groups.First().Count() == 4)
            {
                toReturn = HandStrength.FourOfAKind;
            }
            else
            {
                toReturn = HandStrength.FullHouse;
            }
        }
        else if(groups.Count() == 3)
        {
            if(groups.Any(g => g.Count() == 3))
            {
                toReturn = HandStrength.ThreeOfAKind;
            }
            else
            {
                toReturn = HandStrength.TwoPair;
            }
        }
        else if(groups.Count() == 4)
        {
            toReturn = HandStrength.Pair;
        }
        return toReturn;
    }

    private int GetCardValue(char card)
    {
        int toReturn = 0;
        if(!int.TryParse(card.ToString(), out toReturn))
        {
            toReturn = card switch
            {
                'T' or 't' => 10,
                'J' or 'j' => this.IncludeJoker ? 1 : 11,
                'Q' or 'q' => 12,
                'K' or 'k' => 13,
                'A' or 'a' => 14,
                _ => 0
            };
        }
        return toReturn;
    }

    public int Compare(string hand1, string hand2)
    {
        int toReturn = this.GetHandStrength(hand1).CompareTo(this.GetHandStrength(hand2));
        if(toReturn == 0)
        {
            for(int i = 0; i < hand1.Length; i++) 
            {
                toReturn = this.GetCardValue(hand2[i]).CompareTo(this.GetCardValue(hand1[i]));
                if(toReturn != 0)
                {
                    break;
                }
            }
        }
        return toReturn;
    }
}