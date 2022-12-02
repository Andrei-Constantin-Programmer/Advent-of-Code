using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code
{
    internal class Utilities
    {
        public static TextReader GetInputFile(int year, int day)
        {
            return File.OpenText(GetFileString("input", year, day));
        }

        public static StreamWriter GetOutputFile(int year, int day)
        {
            return new StreamWriter(GetFileString("output", year, day));
        }

        private static string GetFileString(string folder, int year, int day)
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @$"\resources\{folder}\{year}_{day}.txt";
        }

    }
}
