using System.Runtime.CompilerServices;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution8 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            int rows, columns;
            var grid = ReadTreeGrid(out rows, out columns);

            int perimeter = CalculatePerimeter(rows, columns);
            int visibleTrees = GetVisibleTreeCount(grid, rows, columns);

            Console.WriteLine(perimeter + visibleTrees);
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private int GetVisibleTreeCount(byte[][] grid, int rows, int columns)
        {
            int count = 0;
            for(int i = 1; i < rows - 1; i++)
            {
                for(int j = 1; j < columns - 1; j++)
                {
                    if(TreeIsVisible(grid, rows, columns, (i, j)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool TreeIsVisible(byte[][] grid, int rows, int columns, (int row, int column) treePosition)
        {
            return TreeVisibleFromTop(grid, rows, columns, treePosition)
                || TreeVisibleFromBottom(grid, rows, columns, treePosition)
                || TreeVisibleFromLeft(grid, rows, columns, treePosition)
                || TreeVisibleFromRight(grid, rows, columns, treePosition);
        }

        private bool TreeVisibleFromTop(byte[][] grid, int rows, int columns, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            for (var i = treePosition.row - 1; i >= 0; i--)
            {
                if (treeHeight <= grid[i][treePosition.column])
                {
                    return false;
                }
            }

            return true;
        }

        private bool TreeVisibleFromBottom(byte[][] grid, int rows, int columns, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            for (var i = treePosition.row + 1; i < rows; i++)
            {
                if (treeHeight <= grid[i][treePosition.column])
                {
                    return false;
                }
            }

            return true;
        }

        private bool TreeVisibleFromLeft(byte[][] grid, int rows, int columns, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            for (var j = treePosition.column - 1; j >= 0; j--)
            {
                if (treeHeight <= grid[treePosition.row][j])
                {
                    return false;
                }
            }

            return true;
        }

        private bool TreeVisibleFromRight(byte[][] grid, int rows, int columns, (int row, int column) treePosition)
        {
            var treeHeight = grid[treePosition.row][treePosition.column];

            for (var j = treePosition.column + 1; j < columns; j++)
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

        private byte[][] ReadTreeGrid(out int rows, out int columns)
        {
            var lines = File.ReadAllLines(Utilities.GetFileString(Utilities.FileType.Input, 2022, 8));
            rows = lines.Length;
            columns = lines[0].Length;

            var grid = new byte[rows][];

            for(byte i = 0; i < rows; i++)
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
