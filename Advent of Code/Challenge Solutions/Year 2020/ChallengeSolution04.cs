// Task: https://adventofcode.com/2020/day/4

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020;

public class ChallengeSolution04 : ChallengeSolution
{
    private string text;
    private string[] passports;
    private static string[] reqFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
    private static string[] eyeColors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };


    public ChallengeSolution04(IConsole console) : base(console)
    {
        text = string.Join(Environment.NewLine, Reader.ReadLines(this));
        passports = text.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }

    public override void SolveFirstPart()
    {
        Console.WriteLine(ValidPassports(passports));
    }

    public override void SolveSecondPart()
    {
        Console.WriteLine(ValidPassportsValidated(passports));
    }

    private static int ValidPassports(string[] passports)
    {
        int valid = 0;

        foreach (string passport in passports)
        {
            bool isValid = true;
            foreach (string reqField in reqFields)
            {
                if (!passport.Contains(reqField + ":"))
                    isValid = false;
            }
            if (isValid)
                valid++;
        }

        return valid;
    }

    private static int ValidPassportsValidated(string[] passports)
    {
        int valid = 0;

        foreach (string passport in passports)
        {
            bool isValid = true;
            int nr = 0;

            string[] lines = passport.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string[] fields = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string field in fields)
                {
                    string[] values = field.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (!values[0].Equals("cid"))
                    {
                        nr++;

                        if (!Validate(values[0], values[1]))
                            isValid = false;
                    }
                }
            }
            //Console.WriteLine(nr);
            if (nr <= reqFields.Length - 1)
                isValid = false;
            if (isValid)
                valid++;
        }

        return valid;
    }

    private static bool Validate(string field, string value)
    {
        bool valid = true;
        switch (field)
        {
            case "byr":
                if (!(value.Length == 4 && Convert.ToInt32(value) >= 1920 && Convert.ToInt32(value) <= 2002))
                {
                    valid = false;
                }
                break;
            case "iyr":
                if (!(value.Length == 4 && Convert.ToInt32(value) >= 2010 && Convert.ToInt32(value) <= 2020))
                {
                    valid = false;
                }
                break;
            case "eyr":
                if (!(value.Length == 4 && Convert.ToInt32(value) >= 2020 && Convert.ToInt32(value) <= 2030))
                {
                    valid = false;
                }
                break;
            case "hgt":
                valid = ValidateHeight(value);
                break;
            case "hcl":
                valid = ValidateHairColor(value);
                break;
            case "ecl":
                if (!eyeColors.Contains(value))
                    valid = false;
                break;
            case "pid":
                if (value.Length != 9)
                    valid = false;
                break;
            default:
                break;
        }
        return valid;
    }

    private static bool ValidateHeight(string code)
    {
        string measurement = "" + code[code.Length - 2] + code[code.Length - 1];
        if (measurement.Equals("in"))
        {
            code = code.Substring(0, code.Length - 2);
            int value = Convert.ToInt32(code);
            return (value >= 59 && value <= 76);
        }
        else if (measurement.Equals("cm"))
        {
            code = code.Substring(0, code.Length - 2);
            int value = Convert.ToInt32(code);
            return (value >= 150 && value <= 193);
        }
        else
            return false;
    }

    private static bool ValidateHairColor(string code)
    {
        if (code[0] != '#' || code.Length != 7)
            return false;
        for (int i = 1; i < code.Length; i++)
            if (!(code[i] >= '0' && code[i] <= '9' || code[i] >= 'a' && code[i] <= 'f'))
                return false;
        return true;
    }
}
