// Task: https://adventofcode.com/2021/day/15

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution15(IConsole console, ISolutionReader<ChallengeSolution15> reader)
    : ChallengeSolution<ChallengeSolution15>(console, reader)
{
    private int n;
    private int[,] riskMatrix;
    private int[,] pathMatrix;
    Queue<KeyValuePair<int, int>> queue;

    public override void SolveFirstPart()
    {
        ReadMatrix();
        FindPath();
    }

    public override void SolveSecondPart()
    {
        CreateBigMatrix();
        FindPath();
    }

    private void FindPath()
    {
        queue = new Queue<KeyValuePair<int, int>>();
        pathMatrix = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                pathMatrix[i, j] = -1; //Infinity
        pathMatrix[0, 0] = 0;
        queue.Enqueue(new KeyValuePair<int, int>(0, 0));
        while (queue.Count > 0)
        {
            var elem = queue.Dequeue();
            CalculatePathsFrom(elem.Key, elem.Value);
        }

        Console.WriteLine(pathMatrix[n - 1, n - 1]);
    }

    private void ReadMatrix()
    {
        string[] lines = Reader.ReadLines();
        n = lines.Length;
        riskMatrix = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            int[] values = Array.ConvertAll(lines[i].ToCharArray(), character => (int)Char.GetNumericValue(character));
            for (int j = 0; j < n; j++)
                riskMatrix[i, j] = values[j];
        }
    }

    private void CreateBigMatrix()
    {
        ReadMatrix();
        var bigMatrix = new int[5 * n, 5 * n];
        var matrices = new List<int[,]>();
        matrices.Add(riskMatrix);
        for (int i = 1; i <= 8; i++)
            matrices.Add(IncrementMatrix(matrices[i - 1]));

        int startIndex = 0;

        for (int iMatrix = 0; iMatrix < 5; iMatrix++)
        {
            int matrixNo = startIndex;
            for (int jMatrix = 0; jMatrix < 5; jMatrix++)
            {
                for (int i = iMatrix * n; i < iMatrix * n + n; i++)
                    for (int j = jMatrix * n; j < jMatrix * n + n; j++)
                    {
                        bigMatrix[i, j] = matrices[matrixNo][i % n, j % n];
                    }
                matrixNo++;
            }

            startIndex++;
        }

        n = 5 * n;
        riskMatrix = bigMatrix;
    }

    private int[,] IncrementMatrix(int[,] matrix)
    {
        var newMatrix = new int[n, n];

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                newMatrix[i, j] = matrix[i, j] + 1;
                if (newMatrix[i, j] > 9)
                    newMatrix[i, j] = 1;
            }

        return newMatrix;
    }

    private void CalculatePathsFrom(int i, int j)
    {
        if (i - 1 >= 0)
            CalculatePath(i, j, i - 1, j);
        if (i + 1 < n)
            CalculatePath(i, j, i + 1, j);
        if (j - 1 >= 0)
            CalculatePath(i, j, i, j - 1);
        if (j + 1 < n)
            CalculatePath(i, j, i, j + 1);
    }

    private void CalculatePath(int iSource, int jSource, int iDest, int jDest)
    {
        int sourcePath = pathMatrix[iSource, jSource];
        int destPath = pathMatrix[iDest, jDest];
        if (destPath == -1 || sourcePath + riskMatrix[iDest, jDest] < destPath)
        {
            pathMatrix[iDest, jDest] = sourcePath + riskMatrix[iDest, jDest];
            queue.Enqueue(new KeyValuePair<int, int>(iDest, jDest));
        }
    }


    private void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                Console.Write(matrix[i, j] + " ");
            Console.WriteLine();
        }
    }
}
