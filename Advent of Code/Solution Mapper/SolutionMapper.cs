using Advent_of_Code.Challenge_Solutions;
using Advent_of_Code.Utilities;
using System.Reflection;

namespace Advent_of_Code.Solution_Mapper;

internal class SolutionMapper : ISolutionMapper
{
    private const string YEAR_NAMESPACE_PREFIX = "Advent_of_Code.Challenge_Solutions.Year_";
    private readonly List<int> _years;

    private readonly IConsole _console;

    public SolutionMapper(IConsole console)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        _years = currentAssembly.GetTypes()
            .Where(type => type.Namespace is not null && type.Namespace.StartsWith(YEAR_NAMESPACE_PREFIX))
            .Select(type => int.TryParse(type.Namespace!.Split('_').Last(), out int year) ? year : -1)
            .Where(year => year != -1)
            .Distinct()
            .ToList();

        _console = console;
    }

    public bool DoesYearExist(int year) => _years.Contains(year);

    public ChallengeSolution GetChallengeSolution(int year, int day)
    {
        var challengeClassName = $"ChallengeSolution{Reader.FormatDay(day)}";
        var fullClassName = $"{GetChallengeYearNamespace(year)}.{challengeClassName}";

        var type = Type.GetType(fullClassName)
            ?? throw new NonexistentChallengeException(year, day);

        return Activator.CreateInstance(type, _console) is ChallengeSolution solution
            ? solution
            : throw new Exception($"The solution for challenge {year}_{day} is malformed");
    }

    private static string GetChallengeYearNamespace(int year) => $"{YEAR_NAMESPACE_PREFIX}{year}";
}
