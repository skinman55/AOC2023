using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day1
    {

        public static void SolvePart1()
        {
            using (var reader = File.OpenText("dayinput.txt"))
            {

                var result = 0;
                var currentLine = reader.ReadLine() ?? "";

                while (currentLine != "")
                {

                    int? num1 = null;
                    int? num2 = null;

                    foreach (var c in currentLine.ToCharArray())
                    {
                        if (char.IsNumber(c))
                        {
                            num1 ??= int.Parse(c.ToString());
                            num2 = int.Parse(c.ToString());
                        }
                    }

                    
                    result += (int.Parse($"{num1.Value}{num2.Value}"));

                    currentLine = reader.ReadLine() ?? "";
                }

                Console.WriteLine($"part 1 result {result}");
                Console.ReadKey();
            }
        }

        public static void SolvePart2()
        {

            //using (var reader = File.OpenText("temp.txt"))
            using (var reader = File.OpenText("day1input.txt"))
            {

                var result = 0;
                var currentLine = reader.ReadLine() ?? "";

                while (currentLine != "")
                {

                    int? num1 = null;
                    int? num2 = null;

                    var wordbuffer = new StringBuilder();

                    foreach (var c in currentLine.ToCharArray())
                    {
                        if (char.IsNumber(c))
                        {
                            num1 ??= int.Parse(c.ToString());
                            num2 = int.Parse(c.ToString());


                            wordbuffer.Clear();
                        }
                        else
                        {
                            
                            wordbuffer.Append(c);
                            var numVal= StringContainsNumber(wordbuffer.ToString());
                            if(numVal != null)
                            {
                                num1 ??= numVal;
                                num2 = numVal;
                            }
                        }
                    }

                    Console.WriteLine($"{currentLine} {num1} {num2}");
                    result += (int.Parse($"{num1.Value}{num2.Value}"));

                    currentLine = reader.ReadLine() ?? "";
                }

                Console.WriteLine($"part 2 result {result}");
                Console.ReadKey();
            }
        }

        private static readonly Dictionary<string, int> WordNumberMapping = new Dictionary<string, int>
        {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9},
            {"ten", 10},
            {"eleven", 11},
            {"twelve", 12},
            {"thirteen", 13},
            {"fourteen", 14},
            {"fifteen", 15},
            {"sixteen", 16},
            {"seventeen", 17},
            {"eighteen", 18},
            {"nineteen", 19},
            {"twenty", 20},
            {"thirty", 30},
            {"forty", 40},
            {"fifty", 50},
            {"sixty", 60},
            {"seventy", 70},
            {"eighty", 80},
            {"ninety", 90}
        };

        public static int? StringContainsNumber(string word)
        {
            while (word.Length > 0)
            {
                var result = ConvertWordToNumber(word);
                if (result != null)
                {
                    return result;
                }

                word = word.Substring(1);
            }

            return null;
        }

        public static int? ConvertWordToNumber(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return null;
            }

            word = word.ToLower().Trim();

            if (WordNumberMapping.TryGetValue(word, out int value))
            {
                return value;
            }

            string[] parts = word.Split('-');

            if (parts.Length == 2)
            {
                int tensPart, onesPart;
                if (WordNumberMapping.TryGetValue(parts[0], out tensPart)
                    && WordNumberMapping.TryGetValue(parts[1], out onesPart))
                {
                    return tensPart + onesPart;
                }
            }

            return null;
        }

    }
}
