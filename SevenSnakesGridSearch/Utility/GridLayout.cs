using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SevenSnakesGridSearch.Utility
{

    public class GridLayout
    {
        private List<int[]> data;
        private int offset;
        private int? height;
        private int? width;
        private TextReader reader;


        public GridLayout(TextReader reader)
        {
            this.reader = reader;
            data = new List<int[]>();
            ReadNextLine();
        }


        private bool ReadNextLine()
        {
            string line;
            if ((line = reader.ReadLine()) != null)
            {
                //var cells = ParseLine(line);

                string[] linevalues = line.Split(',');
                int valuecount = linevalues.Length;
                List<int> valueholder = new List<int>();
                int i = 0;
                foreach (string s in linevalues)
                {
                    if (!string.IsNullOrWhiteSpace(s.Trim()) && !string.IsNullOrEmpty(s.Trim()))
                    {
                        valueholder.Add(int.Parse(s.Trim()));
                    }
                    i++;
                }
                var cells= valueholder.ToArray();
                //if (width != null && cells.Length != width)
                //{
                //    throw new Exception(string.Format("Invalid csv line length: row {0}", offset + data.Count));
                //}

                //width = cells.Length;
                if(cells!= null)
                {
                    for (var col = 0; col < cells.Length; col++)
                    {
                        if (cells[col] > 256 || cells[col] < 0)
                        {
                            throw new Exception(string.Format("Invalid cell value in row {0} column {1}", offset + data.Count + 1, col + 1));
                        }
                    }
                }else
                {
                    throw new Exception(string.Format("Invalid cell value in row {0} column {1}", offset + data.Count + 1));
                }
                data.Add(cells);
                if (data.Count > SnakeHelper.MAX_LENGTH * 2 - 1)
                {
                    data.RemoveAt(0);
                    offset++;
                }
                return true;
            }

            height = offset + data.Count;
            return false;
        }
        
        public bool isPointInside(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width)
                return false;
            while (y - offset >= data.Count && ReadNextLine())
            {
            }

            return y - offset < data.Count;
        }
        
        public int GetCell(int x, int y)
        {
            return data[y - offset][x];
        }
        public Tuple<SnakeHelper, SnakeHelper> GetMatchingPair()
        {
            var sums = new List<SnakeHelper>[1793];

            var y = 0;
            while (height == null || y < height)
            {
                for (var x = 0; x < width; x++)
                {
                    var snakes = new List<SnakeHelper>
                    {
                        new SnakeHelper(new Tuple<int, int>(x,y), new HashSet<Tuple<int, int>>(), GetCell(x, y))
                    };
                    for (var l = 1; l < SnakeHelper.MAX_LENGTH; l++)
                    {
                        var next = new List<SnakeHelper>();
                        foreach (var snake in snakes)
                        {
                            next.AddRange(snake.GrownList(x, y, this));
                        }
                        snakes.Clear();
                        snakes = next;
                    }

                    foreach (var snake in snakes)
                    {
                        if (sums[snake.Weight] == null)
                        {
                            sums[snake.Weight] = new List<SnakeHelper>();
                        }

                        var maxOverlappedSize = 0;
                        foreach (var oldSnake in sums[snake.Weight])
                        {
                            var overlapped = oldSnake.OverlappedSize(snake);
                            if (overlapped == 0)
                            {
                                return new Tuple<SnakeHelper, SnakeHelper>(oldSnake, snake);
                            }
                            if (overlapped > maxOverlappedSize)
                            {
                                maxOverlappedSize = overlapped;
                            }

                        }

                        if (maxOverlappedSize < SnakeHelper.MAX_LENGTH)
                        {
                            sums[snake.Weight].Add(snake);
                        }
                    }
                }
                y++;
            }

            return null;
        }
    }

}
