using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    class CellsGroup
    {
        private List<Cell> cells;

        public IReadOnlyList<Cell> Cells { get => cells; }

        private CellsGroup()
        {
            cells = new List<Cell>();
        }

        public bool IsDefinite()
        {
            return Cells.All(c => c.IsDefinite());
        }
        public bool MakeExceptions()
        {
            //var res = false;
            //var changed = true;

            //while (changed)
            //{
            //    changed = Cells
            //        .Where(c => c.IsDefinite())
            //        .Aggregate(false,
            //            (r, c) => Cells.Aggregate(r,
            //                (ri, ci) => ci.ExceptFromPossibleValues(c.Value) || ri));

            //    res = res || changed;
            //}

            //var undefinedCells = Cells.Where(c => !c.IsDefinite());

            //foreach (var cell in undefinedCells)
            //{
            //    foreach (byte val in cell.PossibleValues)
            //    {
            //        if (undefinedCells.Where(c => c != cell && c.PossibleValues.Contains(val)).Count() == 0)
            //        {
            //            cell.Value = val;
            //            res = true;
            //            break;
            //        }
            //    }
            //}

            //return res;

            var changed = false;

            var groupValues = Cells.Where(c => c.IsDefinite()).Select(c => c.Value);
            changed = Cells.Max(c => c.ExceptFromPossibleValues(groupValues)) || changed;

            var unfoundValues = Cells.SelectMany(c => c.PossibleValues).Distinct(); // new List<byte> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            foreach (var i in unfoundValues)
            {
                var cells = Cells.Where(c => !c.IsDefinite() && c.PossibleValues.Contains(i));
                
                if (cells.Count() == 1)
                {
                    var vals = unfoundValues.Where(v => v != i);
                    changed = cells.Single().ExceptFromPossibleValues(vals) || changed;
                }
            }

            return changed;
        }

        public static CellsGroup FromIGrouping(IGrouping<int, Cell> group)
        {
            var res = new CellsGroup();
            res.cells = group.ToList();

            return res;
        }
    }
}
