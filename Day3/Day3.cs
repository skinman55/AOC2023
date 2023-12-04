using System.Text;
using System.Text.RegularExpressions;

namespace AOC2023.Day3
{
    internal class Day3
    {
        private List<DataRow> _dataRows;

        public void SolvePart1And2()
        {
            ReadInput();

            var result = 0;

            var sb = new StringBuilder();

            var rowIndex = 0;
            foreach (var row in _dataRows)
            {
                sb.Append($"Row {rowIndex + 1}: ");
                foreach (var number in row.Numbers)
                {
                    if (PartNumberIsValid(number, rowIndex))
                    {
                        sb.Append($"{number.Value}-");
                        sb.Append("T ");
                        result += number.Value;
                    }
                    else
                    {
                        sb.Append($"{number.Value}-");
                        sb.Append("F ");
                    }
                }

                sb.AppendLine("");
                rowIndex++;
            }

            Console.WriteLine($"part 1 result {result}");

            
            //Solve Part 2
            result = 0;

            foreach (var row in _dataRows.Where(x => x.Gears.Any()))
            {
                foreach (var gear in row.Gears.Where(x => x.Value.Count == 2))
                {
                    result += gear.Value[0] * gear.Value[1];
                }
            }

            Console.WriteLine($"part 2 result {result}");

        }


        private void ReadInput()
        {
            _dataRows = new();

            using (var reader = File.OpenText("../../../Day3/day3input.txt"))
            {

                var currentLine = reader.ReadLine() ?? "";

                var specialChars = new List<char>();

                while (currentLine != "")
                {
                    var dr = new DataRow
                    {
                        RawText = currentLine
                    };

                    foreach (var c in currentLine.ToCharArray())
                    {
                        if (!char.IsNumber(c) && c != '.')
                        {
                            if (!specialChars.Contains(c))
                            {
                                specialChars.Add(c);
                            }
                        }
                    }

                    var numberMatches = Regex.Matches(currentLine, "[0-9]{1,3}");
                    var index = -1;
                    foreach (var number in numberMatches)
                    {
                        var numStr = $"{number}";
                        index = currentLine.IndexOf(numStr, index + 1);
                        dr.Numbers.Add(index, int.Parse($"{number}"));
                        index += numStr.Length;
                    }

                    index = -1;
                    var symbolMatches = Regex.Matches(currentLine, "[\\/$&+,:;=?@#|'<>^*()%!-]");
                    foreach (var symbol in symbolMatches)
                    {
                        index = currentLine.IndexOf($"{symbol}", index + 1);
                        dr.Symbols.Add(index, $"{symbol}");
                        if (symbol == "*")
                        {
                            dr.Gears.Add(index, new List<int>());
                        }
                    }

                    _dataRows.Add(dr);

                    currentLine = reader.ReadLine() ?? "";
                }
            }
        }

        private bool PartNumberIsValid(KeyValuePair<int, int> partNumber, int rowIndex)
        {
            var numberWidth = $"{partNumber.Value}".Length;

            //check Current Row
            if (CheckRow(rowIndex, partNumber, numberWidth))
            {
                return true;
            }

            //check Previous Row
            if (rowIndex > 0)
            {
                if (CheckRow(rowIndex - 1, partNumber, numberWidth))
                {
                    return true;
                }
            }

            //check Next Row
            if (rowIndex < _dataRows.Count - 1)
            {
                if (CheckRow(rowIndex + 1, partNumber, numberWidth))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckRow(int rowIndex, KeyValuePair<int, int> partNumber, int numberWidth)
        {
            var symbols = _dataRows[rowIndex].Symbols;

            //check current row
            if (partNumber.Key > 0)
            {
                //check if symbol is before the number position
                if (symbols.ContainsKey(partNumber.Key - 1))
                {
                    var symbol = symbols[partNumber.Key - 1];
                    if (symbol == "*")
                    {
                        AddValueToGear(rowIndex, partNumber.Key - 1, partNumber.Value);
                    }

                    return true;
                }
            }

            //check if symbol is within the number position
            for (var x = partNumber.Key; x < partNumber.Key + numberWidth; x++)
            {
                if (symbols.ContainsKey(x))
                {
                    var symbol = symbols[x];
                    if (symbol == "*")
                    {
                        AddValueToGear(rowIndex, x, partNumber.Value);
                    }
                    return true;
                }
            }

            if (partNumber.Key < _dataRows[rowIndex].RawText.Length)
            {
                //check if symbol is after the number position
                if (symbols.ContainsKey(partNumber.Key + numberWidth))
                {

                    var symbol = symbols[partNumber.Key + numberWidth];
                    if (symbol == "*")
                    {
                        AddValueToGear(rowIndex, partNumber.Key + numberWidth, partNumber.Value);

                    }
                    return true;
                }
            }

            return false;
        }

        private void AddValueToGear(int rowIndex, int symbolKey, int partNumberValue)
        {
            if (_dataRows[rowIndex].Gears.ContainsKey(symbolKey))
            {
                _dataRows[rowIndex].Gears[symbolKey].Add(partNumberValue);
            }
            else
            {
                _dataRows[rowIndex].Gears[symbolKey] = new List<int> { partNumberValue };
            }
        }

        private class DataRow
        {
            public string RawText { get; set; }
            public Dictionary<int, int> Numbers { get; } = new();
            public Dictionary<int, string> Symbols { get; } = new();

            public Dictionary<int, List<int>> Gears { get; } = new();
        }

    }
}
