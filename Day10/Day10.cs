using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023
{

    /******************************************************************************************************************************************
     *
     *
     * FAILED ATTEMPT:  This solution produced correct results for the sample data, but never produced the right answer for the puzzle input
     * Conceded after several hours of attempts
     * Attempt was Inspired by this successful solution:
     * https://github.com/rtrinh3/AdventOfCode/blob/ef2cf6674306807ae186ea21e720673b92633ce7/Aoc2023/Aoc2023Main.cs     *
     *
     *
     *******************************************************************************************************************************************/


    internal class Day10
    {
        private class Coord
        {
            public Coord(int row, int column)
            {
                Row = row;
                Column = column;
            }

         
            public Coord(int row, int column, char value, Orientation orientation)
            {
                Row = row;
                Column = column;
                Value = value;
                Orientation = orientation;
            }

            public int Row { get; set; }
            public int Column { get; set; }
            public char Value { get; set; }
            public Orientation Orientation { get; set; }

            public int Distance { get; set;}

            public override string ToString()
            {
                return $"{Row}, {Column}, {Value}, {Orientation}";
            }

            public override bool Equals(object? obj)
            {

                var item = obj as Coord;

                if (item == null)
                {
                    return false;
                }

                return Row == item.Row && Column == item.Column && item.Distance == Distance;
            }

            public override int GetHashCode()
            {
                return this.Row.GetHashCode() + this.Column.GetHashCode();
            }
        }

        private  char[,] InitData()
        {
            var lines = File.ReadAllLines("temp2.txt");

            var width = lines[0].Length;

            var data = new char[lines[0].Length, lines.Length];

            for (var x = 0; x < lines.Length; x++)
            {
                var line = lines[x].ToCharArray();
                for (var y = 0; y < width; y++)
                {
                    data[x, y] = line[y];
                }
            }

            return data;
        }

        
        private Dictionary<Coord, int> currentPath = new();
        private Dictionary<Coord, int> intersections = new();


        public void Solve()
        {
           var p1Result = 0;
           //var p2Result = 0;

           var data = InitData();
           var start = FindStart(data);

           var neighbors = GetNeighbors(data, start);

           var distance = 0;

           Console.WriteLine($"{start.Row},{start.Column} {'S'} {distance}");

           currentPath = new Dictionary<Coord, int>();

           foreach (var neighbor in neighbors)
           {
               distance = 1;

               if (neighbor.Value == '.')
                   continue;
               Console.WriteLine($"BEGIN PATH{Environment.NewLine}{neighbor.Row},{neighbor.Column} {neighbor.Value} {distance} {neighbor.Orientation}");

               

               FindNextNode(data, neighbor, distance);

              
           }

           var x = currentPath.Count;
           p1Result = currentPath.ElementAt(currentPath.Count - 1).Value + 1;

            Console.WriteLine("Part1 Result: " + p1Result);
           //Console.WriteLine("Part2 Result: " + p2Result);
        }

        private enum Orientation
        {
            North,
            South,
            East,
            West
        }

        private  bool FindNextNode(char[,] data, Coord current, int distance)
        {
            if (distance > 9)
            {
                return true;
            }

            distance++;
            var nextRow = current.Row;
            var nextCol = current.Column;
            Orientation orientation = current.Orientation;

            switch (current.Value)
            {
                case '|':
                    break; 

                case '-':
                    break; 

                case 'L': //north and east
                    switch (current.Orientation)
                    {
                        case Orientation.East:
                        case Orientation.West:
                            orientation = Orientation.North;
                            break;
                        default:
                            orientation = Orientation.East;
                            break;
                    }

                    break; 

                case 'J': // north and west
                    switch (current.Orientation)
                    {
                        case Orientation.East:
                        case Orientation.West:
                            orientation = Orientation.North;
                            break;
                        default:
                            orientation = Orientation.West;
                            break;
                    }

                    break; 
                    
                case '7': //south and west
                 switch (current.Orientation)
                 {
                        case Orientation.East:
                        case Orientation.West:
                            orientation = Orientation.South;
                            break;
                        default:
                            orientation = Orientation.West;
                            break;
                 }
 
                 break; 

                case 'F': //south and east
                    switch (current.Orientation)
                    {
                        case Orientation.East:
                        case Orientation.West:
                            orientation = Orientation.South;
                            break;
                        default:
                            orientation = Orientation.East;
                            break;
                    }
                    break;
            }

            switch (orientation)
            {
                case Orientation.North:
                    nextRow = current.Row - 1;
                    break;
                case Orientation.South:
                    nextRow = current.Row + 1;
                    break;
                case Orientation.East:
                    nextCol = current.Column + 1;
                    break;
                case Orientation.West:
                    nextCol = current.Column - 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var next = new Coord(nextRow, nextCol, GetNodeValue(nextRow, nextCol, data), orientation);
//            Console.WriteLine($"{next.Row},{next.Column} {next.Value} {distance} {next.Orientation}");

            next.Distance = distance;

            if (currentPath.ContainsKey(next))
            {
                intersections.Add(next,distance);
                //return true;
                //intersected previous loop
            }
            else
            {
                currentPath.Add(next, distance);
            }

            return next.Value == 'S' || FindNextNode(data, next, distance);
            
        }

        private  List<Coord> GetNeighbors(char[,] data, Coord current)
        {
            var row = current.Row;
            var column = current.Column;

            var neighbors = new List<Coord>();

            var fourCoords = new List<Coord>
            {
                new(row, column - 1) {Orientation = Orientation.West},
                new(row, column + 1) {Orientation = Orientation.East},
                new(row - 1, column) {Orientation = Orientation.North},
                new(row + 1, column) {Orientation = Orientation.South}
            };

            foreach (var coord in fourCoords)
            {
                row = coord.Row;
                column = coord.Column;
                neighbors.Add(new Coord(row, column ,GetNodeValue(row, column, data), coord.Orientation));
            }
            
            return neighbors;
        }


        private  char GetNodeValue(int row, int column, char[,] data)
        {
            if (row < 0 || row >= data.Length)
            {
                return '.';
            }
            if (column < 0 || column >= data.Length)
            {
                return '.';
            }
            return data[row, column];
        }

        private  Coord FindStart(char[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    if (matrix[row, col] == 'S')
                    {
                        return new Coord(row, col);
                    }
                }
            }

            throw new Exception("Start not found");
        }


        //private  int FindDistance(char[,] matrix, int startX, int startY, int targetX, int targetY)
        //{
        //    int rowCount = matrix.GetLength(0);
        //    int colCount = matrix.GetLength(1);

        //    // Create a 2D array to store the distance from each cell to the starting cell
        //    int[,] distance = new int[rowCount, colCount];

        //    // Initialize the distance array with -1 (indicating unreachable cells)
        //    for (int i = 0; i < rowCount; i++)
        //    {
        //        for (int j = 0; j < colCount; j++)
        //        {
        //            distance[i, j] = -1;
        //        }
        //    }

        //    // Set the distance of the starting cell to 0
        //    distance[startX, startY] = 0;

        //    // Perform flood fill to calculate the distance from the starting cell to each reachable cell
        //    FloodFillWithDistance(matrix, distance, startX, startY);

        //    // Return the distance from the starting cell to the target cell
        //    return distance[targetX, targetY];
        //}

        //private  void FloodFillWithDistance(char[,] matrix, int[,] distance, int row, int col)
        //{
        //    int rowCount = matrix.GetLength(0);
        //    int colCount = matrix.GetLength(1);

        //    if (row < 0 || row >= rowCount || col < 0 || col >= colCount)
        //        return;

        //    if (distance[row, col] != -1)
        //        return;

        //    // Set the distance of the current cell based on the distance of its neighboring cells
        //    if (row > 0 && distance[row - 1, col] != -1)
        //        distance[row, col] = distance[row - 1, col] + 1;
        //    else if (row < rowCount - 1 && distance[row + 1, col] != -1)
        //        distance[row, col] = distance[row + 1, col] + 1;
        //    else if (col > 0 && distance[row, col - 1] != -1)
        //        distance[row, col] = distance[row, col - 1] + 1;
        //    else if (col < colCount - 1 && distance[row, col + 1] != -1)
        //        distance[row, col] = distance[row, col + 1] + 1;

        //    if (distance[row, col] == -1)
        //        return;

        //    // Recursively call flood fill for the neighboring cells
        //    FloodFillWithDistance(matrix, distance, row - 1, col); // up
        //    FloodFillWithDistance(matrix, distance, row + 1, col); // down
        //    FloodFillWithDistance(matrix, distance, row, col - 1); // left
        //    FloodFillWithDistance(matrix, distance, row, col + 1); // right
        //}

    }

}
