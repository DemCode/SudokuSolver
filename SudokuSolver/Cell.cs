using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    class Cell
    {
        private List<byte> possibleValues;

        public byte Value { get; set; }
        public IReadOnlyList<byte> PossibleValues { get => possibleValues; }

        private Cell(byte value = 0)
        {
            Value = value;

            if (value == 0)
            {
                possibleValues = Enumerable.Range(1, 9).Select(i => (byte)i).ToList();
            }
            else
            {
                possibleValues = new List<byte>();
                possibleValues.Add(value);
            }
        }

        public bool IsDefinite()
        {
            return Value > 0;
        }
        public bool ExceptFromPossibleValues(byte value)
        {
            if (IsDefinite())
            {
                return false;
            }

            var excepted = possibleValues.Remove(value);

            if (excepted && possibleValues.Count == 1)
            {
                Value = possibleValues.First();
            }

            return excepted;
        }
        public bool ExceptFromPossibleValues(IEnumerable<byte> values)
        {
            return values.Max(v => ExceptFromPossibleValues(v));
        }

        public static Cell CreateCell(char value)
        {
            return new Cell((byte)("123456789".IndexOf(value) + 1));
        }
    }
}
