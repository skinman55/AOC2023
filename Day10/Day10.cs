namespace AOC2023
{
    internal class Day10
    {
        private  char[,] InitData()
        {
            var lines = File.ReadAllLines("Day10Input.txt");

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


        public void Solve()
        {
            var p1Result = 0;
            //var p2Result = 0;

            var data = InitData();
            var start = FindStart(data);
            var neighbors = GetNeighbors(data, start);

            currentPath = new Dictionary<Coord, int>();

            var loopDistances = new List<int>(0);

            foreach (var neighbor in neighbors)
            {
                var distance = 1;

                if (neighbor.Value == '.')
                    continue;
                //Console.WriteLine(
                //    $"BEGIN PATH {neighbor.Row},{neighbor.Column} {neighbor.Value} {distance} {neighbor.Orientation}");

                distance++;
                var next = FindNextNode(data, neighbor, distance);
                while (true)
                {
                    distance++;

                    next = FindNextNode(data, next, distance);
                    if (currentPath.ContainsKey(next))
                    {
                        loopDistances.Add(distance);
                        break;
                        //intersected previous loop
                    }
                    else
                    {
                        currentPath.Add(next, distance);
                    }

                    if (next.Value == 'S')
                    {
                        //only want half the loop distance for the full loop
                        loopDistances.Add(distance / 2);
                        break;
                    }
                }
            }

            Console.WriteLine("Part1 Result: " + loopDistances.Max());
        }

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

            public int Row { get; }
            public int Column { get; }
            public char Value { get; }
            public Orientation Orientation { get; set; }

            public int Distance { get; set; }

            public override string ToString()
            {
                return $"{Row}, {Column}, {Value}, Distance {Distance}";
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

        private enum Orientation
        {
            North,
            South,
            East,
            West
        }

        private Coord FindNextNode(char[,] data, Coord current, int distance)
        {
           
            //distance++;
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

            //Console.WriteLine($"{next.Row},{next.Column} {next.Value} {distance}");

            var next = new Coord(nextRow, nextCol, GetNodeValue(nextRow, nextCol, data), orientation)
            {
                Distance = distance
            };

            return next;

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
    }
}
