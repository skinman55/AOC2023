
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;

namespace AOC2023
{
    internal class Day7
    {
       
        public static void Solve()
        {

            var races = new Dictionary<double, double>();
            races.Add(7,9);
            races.Add(15, 40); 
            races.Add(30, 200);

           

            int? winResult = null;

            foreach (var race in races)
            {
                var time = race.Key;
                var recordDistance = race.Value;

                var button = 0; //how long to hold the button

                var winCount = 0;
                for (var i = 0; i < time; i++)
                {
                    var distance = button * (time - i);
                    var win = distance > recordDistance;
                    if (win)
                    {
                        winCount++;
                    }
                    
                    button++;
                }

                if (winResult == null)
                {
                    winResult = winCount;
                }
                else
                {
                    winResult *= winCount;
                }
            }

            Console.WriteLine("Result: " + winResult);
        }
    }
}
