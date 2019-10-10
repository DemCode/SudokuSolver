using System;
using System.Collections;
using System.Linq;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var Data =
                //("800000000" +
                // "003600000" +
                // "070090200" +
                // "050007000" +
                // "000045700" +
                // "000100030" +
                // "001000068" +
                // "008500010" +
                // "090000400")
                ("300120000" +
                 "000007000" +
                 "000000700" +
                 "500000309" +
                 "006004800" +
                 "000810250" +
                 "402070005" +
                 "600900000" +
                 "010000080")
                 .Select((c) => Cell.CreateCell(c)).ToList();

            var vLines = Data
                .Select((c, i) => new { Char = c, Index = i })
                .GroupBy((ci) => ci.Index / 9, (ci) => ci.Char)
                .Select((g) => CellsGroup.FromIGrouping(g)).ToList();
            var hLines = Data
                .Select((c, i) => new { Char = c, Index = i })
                .GroupBy((ci) => ci.Index % 9, (ci) => ci.Char)
                .Select((g) => CellsGroup.FromIGrouping(g)).ToList();
            var squares = Data
                .Select((c, i) => new { Char = c, Index = i })
                .GroupBy((ci) => (ci.Index / 9 / 3 * 3) + (ci.Index % 9 / 3), (ci) => ci.Char)
                .Select((g) => CellsGroup.FromIGrouping(g)).ToList();

            var allGroups = vLines.Concat(hLines).Concat(squares);

            Console.WriteLine("Possible commands: exit, print, iter, solve");

            while (true)
            {
                var input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }
                else if (input == "print")
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int e = 0; e < 3; e++)
                        {
                            for (int r = 0; r < 3; r++)
                            {
                                for (int i = 0; i < 9; i++)
                                {
                                    for (int j = 1; j < 4; j++)
                                    {
                                        Cell cell = Data[i + e * 9 + k * 27];

                                        if (cell.IsDefinite())
                                        {
                                            Console.Write(cell.Value);
                                        }
                                        else
                                        {
                                            byte val = (byte)(j + r * 3);
                                            Console.Write(cell.PossibleValues.Contains(val) ? val.ToString() : " ");
                                        }

                                        Console.Write(" ");
                                    }

                                    Console.Write(" ");
                                }

                                Console.WriteLine();
                            }

                            Console.WriteLine();
                        } 
                    }
                }
                else if (input == "iter")
                {
                    foreach (var group in allGroups)
                    {
                        group.MakeExceptions();
                    }
                }
                else if (input == "solve")
                {
                    while (allGroups.Any((g) => g.MakeExceptions())) ;
                }
            }
        }
    }
}
