using System.Runtime.CompilerServices;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution8 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            var grid = ReadTreeGrid();

            int perimeter = CalculatePerimeter(grid.Length, grid[0].Length);
            int visibleTrees = GetVisibleTreeCount(grid);

            Console.WriteLine(perimeter + visibleTrees);
        }

        public void SolveSecondPart()
        {
            var grid = ReadTreeGrid();

            Console.WriteLine(GetScenicScores(grid).Max());
        }

        private List<int> GetScenicScores(byte[][] grid)
        {
            var scores = new List<int>();

            for(int i = 0; i < grid.Length; i++)
            {
                for(int j = 0; j < grid[0].Length; j++)
                {
                    scores.Add(CalculateScenicScore(grid, (i, j)));
                }
            }

            return scores;
        }

        private int CalculateScenicScore(byte[][] grid, (int row, int column) treePosition)
        {
            return TreesVisibleToTop(grid, treePosition)
                 * TreesVisibleToBottom(grid, treePosition)
                 * TreesVisibleToLeft(grid, treePosition)
                 * TreesVisibleToRight(grid, treePosition);
        }

        private int TreesVisibleToTop(byte[][] grid, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            int visible = 0;
            for (int i = treePosition.row - 1; i >= 0; i--)
            {
                visible++;

                if (treeHeight <= grid[i][treePosition.column])
                {
                    break;
                }
            }

            return visible;
        }

        private int TreesVisibleToBottom(byte[][] grid, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];
            
            int visible = 0;
            for (int i = treePosition.row + 1; i < grid.Length; i++)
            {
                visible++;

                if (treeHeight <= grid[i][treePosition.column])
                {
                    break;
                }
            }

            return visible;
        }

        private int TreesVisibleToLeft(byte[][] grid, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            int visible = 0;
            for (int j = treePosition.column - 1; j >= 0; j--)
            {
                visible++;

                if (treeHeight <= grid[treePosition.row][j])
                {
                    break;
                }
            }

            return visible;
        }

        private int TreesVisibleToRight(byte[][] grid, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            int visible = 0;
            for (int j = treePosition.column + 1; j < grid[0].Length; j++)
            {
                visible++;

                if (treeHeight <= grid[treePosition.row][j])
                {
                    break;
                }
            }

            return visible;
        }


        private int GetVisibleTreeCount(byte[][] grid)
        {
            int count = 0;
            for(int i = 1; i < grid.Length - 1; i++)
            {
                for(int j = 1; j < grid[0].Length - 1; j++)
                {
                    if(TreeIsVisible(grid, (i, j)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool TreeIsVisible(byte[][] grid, (int row, int column) treePosition)
        {
            return TreeVisibleFromTop(grid, treePosition)
                || TreeVisibleFromBottom(grid, treePosition)
                || TreeVisibleFromLeft(grid, treePosition)
                || TreeVisibleFromRight(grid, treePosition);
        }

        private bool TreeVisibleFromTop(byte[][] grid, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            for (int i = treePosition.row - 1; i >= 0; i--)
            {
                if (treeHeight <= grid[i][treePosition.column])
                {
                    return false;
                }
            }

            return true;
        }

        private bool TreeVisibleFromBottom(byte[][] grid, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            for (int i = treePosition.row + 1; i < grid.Length; i++)
            {
                if (treeHeight <= grid[i][treePosition.column])
                {
                    return false;
                }
            }

            return true;
        }

        private bool TreeVisibleFromLeft(byte[][] grid, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            for (int j = treePosition.column - 1; j >= 0; j--)
            {
                if (treeHeight <= grid[treePosition.row][j])
                {
                    return false;
                }
            }

            return true;
        }

        private bool TreeVisibleFromRight(byte[][] grid, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            for (int j = treePosition.column + 1; j < grid[0].Length; j++)
            {
                if (treeHeight <= grid[treePosition.row][j])
                {
                    return false;
                }
            }

            return true;
        }

        private int CalculatePerimeter(int rows, int columns)
        {
            return 2 * (rows + columns) - 4;
        }

        private byte[][] ReadTreeGrid()
        {
            var lines = File.ReadAllLines(Utilities.GetFileString(Utilities.FileType.Input, 2022, 8));
            var rows = lines.Length;
            var columns = lines[0].Length;

            var grid = new byte[rows][];

            for(int i = 0; i < rows; i++)
            {
                grid[i] = new byte[columns];
                for(int j = 0; j < columns; j++)
                {
                    grid[i] = lines[i].Select(c => (byte)(c - '0')).ToArray();
                }
            }

            return grid;
        }
    }
}
