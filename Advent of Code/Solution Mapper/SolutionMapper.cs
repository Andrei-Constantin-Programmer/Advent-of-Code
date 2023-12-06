using Advent_of_Code.Challenge_Solutions;
using System.Reflection;

namespace Advent_of_Code.Solution_Mapper;

internal class SolutionMapper : ISolutionMapper
{
    private const string YEAR_NAMESPACE_PREFIX = "Advent_of_Code.Challenge_Solutions.Year_";
    private readonly List<int> _years;

    public SolutionMapper()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        _years = currentAssembly.GetTypes()
            .Where(type => type.Namespace is not null && type.Namespace.StartsWith(YEAR_NAMESPACE_PREFIX))
            .Select(type => int.TryParse(type.Namespace!.Split('_').Last(), out int year) ? year : -1)
            .Where(year => year != -1)
            .Distinct()
            .ToList();
    }

    public bool DoesYearExist(int year) => _years.Contains(year);

    public ChallengeSolution GetChallengeSolution(int year, int day)
    {
        string challengeClassName = $"ChallengeSolution{(day < 10 ? $"0{day}" : $"{day}")}";
        string fullClassName = $"{GetChallengeYearNamespace(year)}.{challengeClassName}";

        var type = Type.GetType(fullClassName);
        if (type is null)
        {
            throw new NonexistentChallengeException(year, day);
        }

        return Activator.CreateInstance(type) is ChallengeSolution solution
            ? solution
            : throw new Exception($"The solution for challenge {year}_{day} is malformed");
    }

    private static string GetChallengeYearNamespace(int year) => $"{YEAR_NAMESPACE_PREFIX}{year}";
}
