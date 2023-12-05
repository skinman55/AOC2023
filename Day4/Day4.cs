namespace AOC2023.Day4
{
    internal class Day4
    {

        public static void SolvePart1()
        {
            using (var reader = File.OpenText("../../../Day4/day4Input.txt"))
            {

                var result = 0;
                var currentLine = reader.ReadLine() ?? "";

                while (currentLine != "")
                {

                    var card = ParseCardFromLine(currentLine);

                    result += card.GetPart1Score();

                    currentLine = reader.ReadLine() ?? "";
                }

                Console.WriteLine($"part 1 result {result}");
            }
        }

        public static void SolvePart2()
        {
            using (var reader = File.OpenText("../../../Day4/day4Input.txt"))
            {

                var result = 0;
                var currentLine = reader.ReadLine() ?? "";
                var origCards = new List<Card>();

                while (currentLine != "")
                {
                    origCards.Add(ParseCardFromLine(currentLine));
                    currentLine = reader.ReadLine() ?? "";
                }

                var cardCounts = new int[origCards.Count];
                for (var i = 0; i < origCards.Count; i++) 
                {
                    var card = origCards[i];
                    
                    cardCounts[i] += 1;

                    if (card.NumberOfMatches > 0)
                    {
                        for (int j=0, idx=i+1; j<card.NumberOfMatches && idx <= origCards.Count; j++)
                        {
                            cardCounts[idx++] += cardCounts[i];
                        }
                    }
                }


                foreach (var count in cardCounts)
                {
                    result += count;
                }
                Console.WriteLine($"part 2 result {result}");
            }
        }

        private static Card ParseCardFromLine(string currentLine)
        {
            var numbers = currentLine.Replace("Card ", "").Replace(":", "").Split(" ");

            var card = new Card();
            var firstTime = true;
            var isPlayerNumber = false;
            foreach (var num in numbers)
            {
                if (num == "")
                {
                    continue;
                }

                if (firstTime)
                {
                    card.ID = int.Parse(num);
                    firstTime = false;
                    continue;
                }

                if (num == "|")
                {
                    isPlayerNumber = true;
                    continue;
                }

                if (isPlayerNumber)
                {
                    card.PlayerNumbers.Add(int.Parse(num));
                }
                else
                {
                    card.WinningNumbers.Add(int.Parse(num));
                }
            }

            return card;

        }


        private class Card
        {
            public int ID { get; set; }
            public List<int> WinningNumbers { get; } = new();
            public List<int> PlayerNumbers { get; } = new();

            public int GetPart1Score()
            {
                if (NumberOfMatches == 0)
                { return 0; }

                var res = 1;
                for (var x = 1; x < NumberOfMatches; x++)
                {
                    res *= 2;
                }

                return res;
            }

            public int NumberOfMatches => PlayerNumbers.Count(x => WinningNumbers.Contains(x));

            public override string ToString()
            {
                return $"Card {ID}";
            }
        }

    }
}
