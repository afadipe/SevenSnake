using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenSnakesGridSearch.Utility
{
   public class SnakeHelper
    {
        public const int MAX_LENGTH = 7;
        private readonly int[] CellX = { 0, 1, 0, -1 };
        private readonly int[] CellY = { 1, 0, -1, 0 };
        public int Weight { get; }
        private Tuple<int, int> Head { get; }
        private HashSet<Tuple<int, int>> Body { get; }
        public SnakeHelper(Tuple<int, int> head, IEnumerable<Tuple<int, int>> body, int weight)
        {
            Head = head;
            Weight = weight;
            Body = new HashSet<Tuple<int, int>>(body) { head };
        }
        
        public IEnumerable<SnakeHelper> GrownList(int X, int Y, GridLayout grid)
        {
            List<SnakeHelper> snakes = new List<SnakeHelper>();
            for (var i = 0; i < 4; i++)
            {
                var head = new Tuple<int, int>(Head.Item1 + CellX[i], Head.Item2 + CellY[i]);
                if (grid.isPointInside(head.Item1, head.Item2)
                    && !Body.Contains(head)
                    && NotAdjacentToBody(head)
                    && MAX_LENGTH - Body.Count - 1 - (head.Item1 > X ? 0 : 1) >= Y - head.Item2
                    )
                    {
                        snakes.Add(new SnakeHelper(head, Body, Weight + grid.GetCell(head.Item1, head.Item2)));
                    }
            }
            return snakes;
        }

        
        private bool NotAdjacentToBody(Tuple<int, int> cell)
        {
            for (var i = 0; i < 4; i++)
            {
                var adjacentcellLayout = new Tuple<int, int>(cell.Item1 + CellX[i], cell.Item2 + CellY[i]);
                if (!adjacentcellLayout.Equals(Head) && Body.Contains(adjacentcellLayout))
                {
                    return false;
                }
            }

            return true;
        }
        
        public int OverlappedSize(SnakeHelper snakemodel)
        {
            return snakemodel.Body.Count(cell => Body.Contains(cell));
        }

        public string DisplayBoardFormatHelper()
        {
            return string.Join(" ", from cell in Body select string.Format("({0},{1})", cell.Item1 + 1, cell.Item2 + 1));
        }
    }
}
