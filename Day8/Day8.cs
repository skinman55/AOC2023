using System.Text.RegularExpressions;

namespace AOC2023
{
    internal class Day8
    {
        private class Coords
        {
            public Coords(string x, string y)
            {
                this.x = x;
                this.y = y;
            }

            public string x { get; set; }
            public string y { get; set; }

            public override string ToString()
            {
                return $"{x},{y}";
            }
        }
        
        private static long FindStepsToNode(char[] instructions, Dictionary<string, Coords> coords, string start, string end)
        {
            long count = 0;
            long curr = 0;
            while (!start.EndsWith(end))
            {
                start = instructions[curr] == 'L' ? coords[start].x : coords[start].y;
                curr = (curr + 1) % instructions.Length;
                ++count;
            }

            return count;
        }

        public static long FindLCM(long a, long b)
        {
            return (a * b) / FindGCD(a, b);
        }

        public static long FindGCD(long a, long b)
        {
            while (b != 0)
            {
                long remainder = a % b;
                a = b;
                b = remainder;
            }

            return a;
        }

        public static void Solve()
        {
            var coords = new Dictionary<string, Coords>();

            var lines = File.ReadAllLines("Day8Input.txt");

            var instructions = lines[0].ToCharArray();

            for (var idx = 2; idx < lines.Length; idx++)
            {
                var line = lines[idx];
                var sections = Regex.Matches(line, "[A-Z1-9][A-Z1-9][A-Z1-9]");
                coords.Add(sections[0].ToString(), new Coords(sections[1].ToString(), sections[2].ToString()));
            }

            var p1 = FindStepsToNode(instructions, coords, "AAA", "ZZZ");


            Console.WriteLine("Part1 Result: " + p1);

            var p2 = coords.Keys.Where(k => k[2] == 'A')
                .Select(p => FindStepsToNode(instructions, coords, p, "Z"))
                .Aggregate(FindLCM);

            Console.WriteLine("Part2 Result: " + p2);

        }

    }

}
