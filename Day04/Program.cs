(int id, IEnumerable<int> winningNumbers, IEnumerable<int> selectedNumbers) parseLine(string line)
{
    (int id, IEnumerable<int> winningNumbers, IEnumerable<int> selectedNumbers) toReturn = (id: 0, winningNumbers: new List<int>(), selectedNumbers: new List<int>());

    if(!string.IsNullOrEmpty(line))
    {
        string[] splitGame = line.Split(':', StringSplitOptions.TrimEntries);
        if (splitGame.Length > 1)
        {
            toReturn.id = getIdFromSplit(splitGame[0]);
            string[] splitNumbers = splitGame[1].Split("|", StringSplitOptions.TrimEntries);
            if(splitNumbers.Length > 1)
            {
                toReturn.winningNumbers = getNumbers(splitNumbers[0]);
                toReturn.selectedNumbers = getNumbers(splitNumbers[1]);
            }
        }
    }

    return toReturn;
}

int getScoreOfelectedWinningNumbers(IEnumerable<int> selectedNumbers, IEnumerable<int> winningNumbers)
{
    int toReturn = 0;
    foreach(int i in selectedNumbers)
    {
        if(winningNumbers.Contains(i))
        {
            if(toReturn == 0)
            {
                toReturn = 1;
            }
            else
            {
                toReturn = toReturn * 2;
            }
        }
    }
    return toReturn;
}

int getIdFromSplit(string stringSplit) => int.Parse(stringSplit.Replace("Card", "").Trim());
IEnumerable<int> getNumbers(string input) => input.Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x.Trim())).ToList();

int part1Sum = File.ReadAllLines("input.txt")
    .Select(l => parseLine(l))
    .ToList()
    .Select(x => getScoreOfelectedWinningNumbers(x.selectedNumbers, x.winningNumbers))
    .Sum();

IDictionary<int, int> cardCount = new Dictionary<int, int>();
IEnumerable<(int id, IEnumerable<int> winningNumbers, IEnumerable<int> selectedNumbers)> cards = File.ReadAllLines("input.txt")
    .Select(l => parseLine(l))
    .ToList();

void addCardCount(int id)
{
    if (!cardCount.ContainsKey(id))
    {
        cardCount[id] = 1;
    }
    else
    {
        cardCount[id]++;
    }
}

foreach ((int id, IEnumerable<int> winningNumbers, IEnumerable<int> selectedNumbers) card in cards)
{
    addCardCount(card.id);
    for(int idx = 0; idx < cardCount[card.id]; idx++)
    {
        int cardIdx = card.id;
        foreach (int i in card.selectedNumbers)
        {
            if (card.winningNumbers.Contains(i))
            {
                cardIdx++;
                addCardCount(cardIdx);
            }
        }
    }
}

int part2Sum = cardCount.Sum(kv => kv.Value);

Console.WriteLine($"Part 1: {part1Sum}");
Console.WriteLine($"Part 2: {part2Sum}");
Console.ReadLine();