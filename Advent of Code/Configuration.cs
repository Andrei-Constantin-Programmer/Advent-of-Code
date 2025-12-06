using Advent_of_Code.ChallengeReader;
using Advent_of_Code.FSharp;
using Advent_of_Code.Services;
using Advent_of_Code.Shared.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Advent_of_Code;

public static class Configuration
{
    internal static IServiceCollection ConfigureServices(this IServiceCollection services) => services
        .AddTransient<IConsole, SystemConsole>()
        .AddTransient(typeof(ISolutionReader<>), typeof(SolutionReader<>))
        .AddKeyedTransient<ISolutionMapper, SolutionMapper>(CSharpLang.ServiceKey)
        .AddKeyedTransient<ISolutionMapper, FSharpSolutionMapper.FSharpSolutionMapper>(FSharpLang.ServiceKey)
        .AddTransient<IChallengeReader, ChallengeReader.ChallengeReader>()
        .AddSingleton<AdventOfCodeApplication>();
}