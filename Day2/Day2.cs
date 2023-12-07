namespace AOC2023
{
    internal class Day2
    {

        public static void SolvePart1()
        {
            using (var reader = File.OpenText("../../../Day2/day2input.txt"))
            {

                var result = 0;
                var currentLine = reader.ReadLine() ?? "";

                while (currentLine != "")
                {

                    var game = ParseGameFromLine(currentLine);

                    if (game.IsPossible)
                    {
                        result += game.ID;
                    }


                    currentLine = reader.ReadLine() ?? "";
                }

                Console.WriteLine($"part 1 result {result}");
            }
        }


        public static void SolvePart2()
        {
            using (var reader = File.OpenText("../../../Day2/day2input.txt"))
            {

                var result = 0;
                var currentLine = reader.ReadLine() ?? "";

                while (currentLine != "")
                {

                    result += GetGameValue(currentLine);


                    currentLine = reader.ReadLine() ?? "";
                }

                Console.WriteLine($"part 2 result {result}");
            }
        }


        public static int GetGameValue(string line)
        {
            var lineSplit = line.Split(':').Select(x => x.Trim()).ToList();
            var game = lineSplit[1];
            var tosses = game.Split(';').Select(x => x.Trim());

            int[] minNumbers = { 0, 0, 0 };
            const string rgb = "rgb";

            foreach (var toss in tosses)
            {
                var throwColors = toss.Contains(',') ? toss.Split(',').Select(x => x.Trim()) : new[] { toss };

                foreach (var currentColor in throwColors)
                {
                    var split = currentColor.Split(' ');
                    char color = split[1][0];
                    int minIndex = rgb.IndexOf(color);
                    int throwNumber = int.Parse(split[0]);

                    if (throwNumber > minNumbers[minIndex])
                    {
                        minNumbers[minIndex] = throwNumber;
                    }
                }
            }

            return minNumbers.Aggregate((x, y) => x * y);
        }


        private static Game ParseGameFromLine(string currentLine)
        {

            var lineSplit = currentLine.Split(':').Select(x => x.Trim()).ToList();
            var gameData = lineSplit[1];
            var gameID = lineSplit[0].Replace("Game ", "").Replace(": ", "");
            var game = new Game { ID = int.Parse(gameID) };
            var tosses = gameData.Split(';').Select(x => x.Trim());

            int[] minNumbers = { 0, 0, 0 };
            const string rgb = "rgb";

            foreach (var toss in tosses)
            {
                var throwColors = toss.Contains(',') ? toss.Split(',').Select(x => x.Trim()) : new[] { toss };

                foreach (var currentColor in throwColors)
                {
                    var split = currentColor.Split(' ');
                    var color = split[1];
                    int throwNumber = int.Parse(split[0]);

                    switch (color)
                    {
                        case "red":
                            if (throwNumber > game.Red)
                            {
                                game.Red = throwNumber;
                            }

                            break;
                        case "green":
                            if (throwNumber > game.Green)
                            {
                                game.Green = throwNumber;
                            }
                            break;
                        case "blue":
                            if (throwNumber > game.Blue)
                            {
                                game.Blue = throwNumber;
                            }
                            break;
                    }
                }
            }



            return game;
        }



        private class Game
        {
            public int ID { get; init; }
            public int Red { get; set; }
            public int Green { get; set; }
            public int Blue { get; set; }

            public bool IsPossible => Red <= 12 && Green <= 13 && Blue <= 14;
            public int TotalCubes => Green + Red + Blue;
        }

    }
}
