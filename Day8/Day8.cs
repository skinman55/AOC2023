
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Security.AccessControl;
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

        public static void Solve()
        {

            var lines = File.ReadAllLines("Day8Input.txt");

            var coords = new Dictionary<string, Coords>();

            var instructions = lines[0];

            for (var idx = 2; idx < lines.Length; idx++)
            {
                var line = lines[idx];
                var sections = Regex.Matches(line, "[A-Z][A-Z][A-Z]");
                coords.Add(sections[0].ToString(), new Coords(sections[1].ToString(), sections[2].ToString()));
            }
            //Part1
            var currentElement = coords["AAA"];
            var result = 1;
            var keepGoing = true;
            while (keepGoing)
            {
                foreach (var instruction in instructions.ToCharArray())
                {
                    var key = "";
                    switch (instruction)
                    {
                        case 'L':
                            key = currentElement.x;
                            break;
                        case 'R':
                            key = currentElement.y;
                            break;
                    }

                    if (key == "ZZZ")
                    {
                        keepGoing = false;
                        break;
                    }

                    currentElement = coords[key];
                    result++;
                }
            }

            Console.WriteLine("Result: " + result);
        }
    }
}
