using Advent_of_Code;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.ConfigureServices();

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetRequiredService<AdventOfCodeApplication>();

app.Run();
