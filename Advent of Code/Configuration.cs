using Advent_of_Code.ChallengeReader;
using Advent_of_Code.Solution_Mapper;
using Advent_of_Code.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Advent_of_Code;

public static class Configuration
{
    internal static IServiceCollection ConfigureServices(this IServiceCollection services) => services
        .AddTransient<IConsole, SystemConsole>()
        .AddTransient(typeof(ISolutionReader<>), typeof(SolutionReader<>))
        .AddTransient<IChallengeReader, ChallengeReader.ChallengeReader>()
        .AddTransient<ISolutionMapper, SolutionMapper>()
        .AddSingleton<AdventOfCodeApplication>()
        ;
}
