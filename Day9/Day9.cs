using System;
using System.Data;
using System.Text.RegularExpressions;

namespace AOC2023
{
    internal class Day9
    {
        public static void Solve()
        {
            
            var lines = File.ReadAllLines("Day9Input.txt");

            var p1Result = 0;
            var p2Result = 0;


            foreach (var line in lines)
            {
                var sa = line.Split(" ");

                var current = sa.Select(int.Parse).ToList();
                var data = new List<int[]>{current.ToArray()};

                var keepGoing = true;
                while (keepGoing)
                {
                    var diffs = new List<int>();

                    for (var i = 1; i < current.Count; i++)
                    {
                        var diff = current[i] - current[i - 1];
                        diffs.Add(diff);
                    }

                    data.Add(diffs.ToArray());
                    
                    current = diffs;

                    if (diffs.All(x => x == 0))
                    {
                        keepGoing = false;
                    }
                }
                //part1

                //add a new zero to the end of your list of zeroes
                var lastRow = data[^1];
                data[^1] = new int[lastRow.Length + 1];

                for (var j = data.Count-1; j > 0; j--)
                {
                    var newRow = new int[data[j - 1].Length + 1];
                    Array.Copy(data[j-1], newRow, data[j-1].Length);
                    var n = data[j][data[j - 1].Length - 1] + data[j-1][data[j - 1].Length - 1];
                    newRow[^1] = n;
                    data[j -1] = newRow;

                }
                p1Result += (data[0][data[0].Length-1]);

                //part2
                data[^1] = new int[lastRow.Length + 1];

                //add a new zero to the beginning of your list of zeroes
                lastRow = data[^1];
                data[^1] = new int[lastRow.Length + 1];

                for (var j = data.Count - 1; j > 0; j--)
                {
                    var newRow = new int[data[j - 1].Length + 1];

                    var n = data[j - 1][0] - data[j][0];
                    newRow[0] = n;

                    Array.Copy(data[j - 1], 0, newRow,1, data[j - 1].Length);

                    data[j - 1] = newRow;

                }
                p2Result += (data[0][0]);
            }
           
            Console.WriteLine("Part1 Result: " + p1Result);
            Console.WriteLine("Part2 Result: " + p2Result);
        }
    }

}
