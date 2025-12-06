using System.Reflection;
using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Advent_of_Code.Services;

internal class SolutionMapper : ISolutionMapper
{
    private const string YearNamespacePrefix = "Advent_of_Code.Challenge_Solutions.Year_";

    private readonly IConsole _console;
    private readonly IServiceProvider _serviceProvider;
    private readonly List<int> _years;

    public SolutionMapper(IConsole console, IServiceProvider serviceProvider)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        _years = currentAssembly.GetTypes()
            .Where(type => type.Namespace is not null && type.Namespace.StartsWith(YearNamespacePrefix))
            .Select(type => int.TryParse(type.Namespace!.Split('_').Last(), out var year) ? year : -1)
            .Where(year => year != -1)
            .Distinct()
            .ToList();

        _console = console;
        _serviceProvider = serviceProvider;
    }

    public bool DoesYearExist(int year) => _years.Contains(year);

    public ChallengeSolution GetChallengeSolution(int year, int day)
    {
        var challengeClassName = $"ChallengeSolution{PathUtils.FormatDay(day)}";
        var fullClassName = $"{GetChallengeYearNamespace(year)}.{challengeClassName}";

        var type = Type.GetType(fullClassName)
                   ?? throw new NonexistentChallengeException(year, day);

        var readerType = typeof(ISolutionReader<>).MakeGenericType(type);
        var reader = _serviceProvider.GetRequiredService(readerType);

        var solution = Activator.CreateInstance(type, _console, reader);

        return solution as ChallengeSolution
               ?? throw new Exception($"The solution for challenge {year}_{day} is malformed");
    }

    private static string GetChallengeYearNamespace(int year) => $"{YearNamespacePrefix}{year}";
}