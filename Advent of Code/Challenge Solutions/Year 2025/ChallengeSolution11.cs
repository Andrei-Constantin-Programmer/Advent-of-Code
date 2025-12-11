// Task: https://adventofcode.com/2025/day/11

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution11(IConsole console, ISolutionReader<ChallengeSolution11> reader)
    : ChallengeSolution<ChallengeSolution11>(console, reader)
{
    private const string YourDeviceName = "you";
    private const string OutputDeviceName = "out";

    private const string ServerRackName = "svr";
    private const string DigitalToAnalogConverterName = "dac";
    private const string FastFourierTransformName = "fft";
    
    public override void SolveFirstPart()
    {
        _ = ReadDevices(YourDeviceName, out var startDevice);

        var pathCount = ComputePathCountToOutputDevice(startDevice, OutputDeviceName);

        Console.WriteLine($"Paths to {OutputDeviceName}: {pathCount}");
    }

    public override void SolveSecondPart()
    {
        var devicesByName = ReadDevices(ServerRackName, out var startDevice);

        long pathCount = 0;

        var dacDevice = devicesByName[DigitalToAnalogConverterName];
        var fftDevice = devicesByName[FastFourierTransformName];

        var pathsFromDacToFft = ComputePathCountToOutputDevice(dacDevice, FastFourierTransformName);
        
        if (pathsFromDacToFft > 0)
        {
            var pathsToDac = ComputePathCountToOutputDevice(startDevice, DigitalToAnalogConverterName);
            var pathsToOut = ComputePathCountToOutputDevice(fftDevice, OutputDeviceName);

            pathCount = pathsToDac * pathsFromDacToFft * pathsToOut;
        }
        else
        {
            var pathsFromFftToDac = ComputePathCountToOutputDevice(fftDevice, DigitalToAnalogConverterName);
            if (pathsFromFftToDac > 0)
            {
                var pathsToFft = ComputePathCountToOutputDevice(startDevice, FastFourierTransformName);
                var pathsToOut = ComputePathCountToOutputDevice(dacDevice, OutputDeviceName);

                pathCount = pathsToFft * pathsFromFftToDac * pathsToOut;
            }
        }
        
        Console.WriteLine($"Paths to {OutputDeviceName} through {DigitalToAnalogConverterName} and {FastFourierTransformName}: {pathCount}");
    }
    
    private static long ComputePathCountToOutputDevice(Device startDevice, string endDeviceName)
    {
        Dictionary<Device, long> pathsToOutFromDevice = [];
        
        return ComputePathsFromDevice(startDevice);

        long ComputePathsFromDevice(Device device)
        {
            if (pathsToOutFromDevice.TryGetValue(device, out var paths))
            {
                return paths;
            }

            if (device.Name == endDeviceName)
            {
                return 1;
            }

            paths = device.Outputs.Sum(ComputePathsFromDevice);
            pathsToOutFromDevice.Add(device, paths);
            
            return paths;
        }
    }

    private Dictionary<string, Device> ReadDevices(string startDeviceName, out Device startDevice)
    {
        var lines = Reader.ReadLines();

        Dictionary<string, Device> devicesByName = [];
        Dictionary<string, string[]> deviceNameToOutputs = [];

        foreach (var line in lines)
        {
            var elements = line
                .Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            
            var deviceName = elements[0];
            var outputs = elements[1]
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            
            devicesByName.Add(deviceName, new Device(deviceName));
            deviceNameToOutputs.Add(deviceName, outputs);
        }
        devicesByName.Add(OutputDeviceName, new Device(OutputDeviceName));
        deviceNameToOutputs.Add(OutputDeviceName, []);

        startDevice = devicesByName[startDeviceName];

        foreach (var deviceName in devicesByName.Keys)
        {
            devicesByName[deviceName].Outputs = 
                deviceNameToOutputs[deviceName]
                .Select(d => devicesByName[d])
                .ToHashSet();
        }

        return devicesByName;
    }

    private class Device(string name)
    {
        public string Name { get; } = name;

        public HashSet<Device> Outputs { get; set; } = [];
    }
}
