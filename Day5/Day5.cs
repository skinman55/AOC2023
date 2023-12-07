
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;

namespace AOC2023
{
    internal class Day5
    {
        private AlmanacMap soilMap = new AlmanacMap();
        private AlmanacMap fertilizerMap = new AlmanacMap();
        private AlmanacMap waterMap = new AlmanacMap();
        private AlmanacMap lightMap = new AlmanacMap();
        private AlmanacMap temperatureMap = new AlmanacMap();
        private AlmanacMap humidityMap = new AlmanacMap();
        private AlmanacMap locationMap = new AlmanacMap();

        public async Task Solve()
        {
            using (var reader = File.OpenText("day5Input.txt"))
            {

                var result = 0;
                var currentLine = await reader.ReadLineAsync() ?? "";

                var seeds = new List<double>();

            

                //for part 2
                var seedMap = new Dictionary<double,double>();

                var currentSection = "";

                while (currentLine != null)
                {

                    if (currentLine == "")
                    {
                        currentSection = "";
                        currentLine = await reader.ReadLineAsync();
                        continue;
                    }

                    if (currentLine.StartsWith("seeds:"))
                    {
                        var sa = currentLine.Substring(7).Split(" ");
                        var idx = 0;
                        double currentKey = 0;
                        foreach (var item in sa)
                        {
                            var parsed = double.Parse(item);
                            seeds.Add(parsed);
                            if (idx % 2 == 0)
                            {
                                currentKey = parsed;
                            }
                            else
                            {
                                seedMap[currentKey] = parsed;
                            }
                            idx++;
                        }
                    }

                    if (!char.IsNumber(currentLine[0]))
                    {
                        currentSection = currentLine;
                    }
                    else
                    {
                       switch (currentSection)
                        {
                            case "seed-to-soil map:":
                                parseLine(currentLine, soilMap);
                                break;
                            case "soil-to-fertilizer map:":
                                parseLine(currentLine, fertilizerMap);
                                break;
                            case "fertilizer-to-water map:":
                                parseLine(currentLine, waterMap);
                                break;
                            case "water-to-light map:":
                                parseLine(currentLine, lightMap);
                                break;
                            case "light-to-temperature map:":
                                parseLine(currentLine, temperatureMap);
                                break;
                            case "temperature-to-humidity map:":
                                parseLine(currentLine, humidityMap);
                                break;
                            case "humidity-to-location map:":
                                parseLine(currentLine, locationMap);
                                break;
                        }
                    }
                
                    currentLine = await reader.ReadLineAsync();
                }

                var locations = new List<double>();

                foreach (var seed in seeds)
                {

                    var soil = LookupValue(seed, soilMap);
                    var fertilizer = LookupValue(soil, fertilizerMap);
                    var water = LookupValue(fertilizer, waterMap);
                    var light = LookupValue(water, lightMap);
                    var temperature = LookupValue(light, temperatureMap);
                    var humidity = LookupValue(temperature, humidityMap);
                    var location = LookupValue(humidity, locationMap);

                    locations.Add(location);
                    //Console.WriteLine($"Seed {seed}, soil {soil}, fertilizer {fertilizer}, water {water}, light {light}, temperature {temperature}, humidity {humidity}, location {location}");
                }

                Console.WriteLine($"part 1 result {locations.Min()}");

                locations = new List<double>();
                
                // Create an array of tasks to store the asynchronous operations
                var tasks = new Task[seedMap.Count];

                for(var i = 0; i < seedMap.Count; i++) 
                {
                    int index = i;

                    // Create an array of tasks to store the asynchronous operations
                    tasks[i] = Task.Run(async () =>
                    {
                        locations.Add( await ProcessSeedAsync(index + 1, seedMap.ElementAt(index)));
                    });
                }

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);

                Console.WriteLine($"part 2 result {locations.Min()}");

            }
        }

        private async Task<double> ProcessSeedAsync(int seedIndex, KeyValuePair<double, double> seed)
        {
            var sw = new Stopwatch();
            sw.Start();
             Console.WriteLine($"Processing Seed {seedIndex}");
            double lastProgress = 0;
            var idx = 0;
            double? minLocation = null;

            for (var x = seed.Key; x < seed.Key + seed.Value; x++)
            {
                var soil = LookupValue(x, soilMap);
                var fertilizer = LookupValue(soil, fertilizerMap);
                var water = LookupValue(fertilizer, waterMap);
                var light = LookupValue(water, lightMap);
                var temperature = LookupValue(light, temperatureMap);
                var humidity = LookupValue(temperature, humidityMap);
                var location = LookupValue(humidity, locationMap);

                if (minLocation == null || location < minLocation)
                {
                    minLocation = location;
                }
                //locations.Add(location);

                var progress = Math.Round(idx / seed.Value * 100);

                if (progress > 0 && progress % 10 == 0 && progress > lastProgress)
                {
                    lastProgress = progress;
                    Console.WriteLine($"Seed {seedIndex} Progress {progress}%");
                }

                idx++;

                //Console.WriteLine($"Seed {seed}, soil {soil}, fertilizer {fertilizer}, water {water}, light {light}, temperature {temperature}, humidity {humidity}, location {location}");
            }
            sw.Stop();

            Console.WriteLine($"Processed seed {seedIndex} in {sw.Elapsed.Hours:D2}:{sw.Elapsed.Minutes:D2}:{sw.Elapsed.Seconds:D2}");
            return minLocation.GetValueOrDefault();

        }


        private double LookupValue(double source, AlmanacMap almanacMap)
        {


            var loopIndex = 0;
            double offset = 0;

            foreach (var sourceDict in almanacMap.SourceDict)
            {
                if (source >= sourceDict.Key && source <= (sourceDict.Key + sourceDict.Value - 1))
                {
                    var idx = source - sourceDict.Key + offset;

                    var destDict = almanacMap.DestDict.ElementAt(loopIndex);

                    return idx + destDict.Key - offset;
                }

                offset += sourceDict.Value;
                loopIndex++;

            }

            return source;
            
        }

        private void parseLine(string currentLine, AlmanacMap map)
        {
            var sa = currentLine.Split(" ");
            
            var destinationStart = double.Parse(sa[0]);
            var sourceStart = double.Parse(sa[1]);
            var rangeLength = double.Parse(sa[2]);

            map.SourceDict[sourceStart] = rangeLength;
            map.DestDict[destinationStart] = rangeLength;
        }

        public class AlmanacMap
        {

            public Dictionary<double, double> SourceDict { get; } = new();
            public Dictionary<double, double> DestDict { get; } = new();
        }
       

    }
}
