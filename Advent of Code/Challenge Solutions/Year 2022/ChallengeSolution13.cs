// Task: https://adventofcode.com/2022/day/13

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution13 : ChallengeSolution
    {
        protected override void SolveFirstPart()
        {
            var packetGroups = ReadPacketGroups();

            var rightOrderIndexSum = 0;

            for (int i = 0; i < packetGroups.Count; i++)
            {
                var packetGroup = packetGroups[i];
                if (packetGroup.Item1.CompareTo(packetGroup.Item2) < 0)
                    rightOrderIndexSum += (i + 1);
            }

            Console.WriteLine(rightOrderIndexSum);
        }

        protected override void SolveSecondPart()
        {
            var packets = GetAllPacketsFromPacketGroups(ReadPacketGroups());
            var dividers = CreateDividerPackets(new int[] { 2, 6 });
            packets.AddRange(dividers);

            packets.Sort();

            var product = 1;
            for(int i = 0; i < packets.Count; i++)
            {
                if (packets[i] == dividers[0])
                {
                    product *= (i + 1);
                }
                else if (packets[i] == dividers[1])
                {
                    product *= (i + 1);
                    break;
                }
            }

            Console.WriteLine(product);
        }

        private static List<Packet> CreateDividerPackets(int[] values)
        {
            var dividers = new List<Packet>();

            foreach(var value in values)
            {
                var divider = new Packet(null);
                var dividerInterior = new Packet(divider);
                dividerInterior.AddItem(new PacketInteger(value, dividerInterior));
                divider.AddItem(dividerInterior);

                dividers.Add(divider);
            }

            return dividers;
        }

        private static List<Packet> GetAllPacketsFromPacketGroups(List<(Packet, Packet)> packetGroups)
        {
            var packets = new List<Packet>();
            foreach (var packetGroup in packetGroups)
            {
                packets.Add(packetGroup.Item1);
                packets.Add(packetGroup.Item2);
            }

            return packets;
        }

        private static List<(Packet, Packet)> ReadPacketGroups()
        {
            var lines = string.Join("\n", File.ReadAllLines(Reader.GetFileString(Reader.FileType.Input, 2022, 13)));

            return lines
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(pair => pair
                                .Split("\n")
                                .Select(packet => GetPacket(packet))
                                .ToArray()
                )
                .Select(pair => (pair[0], pair[1]))
                .ToList();
        }

        private static Packet GetPacket(string packetString)
        {
            var currentPacket = new Packet(null);

            for (int i = 0; i < packetString.Length; i++)
            {
                if (i == packetString.Length - 1)
                    return currentPacket!;

                var c = packetString[i];

                if(c == '[')
                {
                    var newPacket = new Packet(currentPacket);
                    currentPacket!.AddItem(newPacket);
                    currentPacket = newPacket;
                }
                else if(c == ']')
                {
                    currentPacket = currentPacket!.Parent;
                }
                else if(char.IsDigit(c))
                {
                    var digits = new List<char>();
                    while(char.IsDigit(c))
                    {
                        digits.Add(c);
                        i++;
                        c = packetString[i];
                    }

                    i--;

                    currentPacket!.AddItem(new PacketInteger(Convert.ToInt32(new string(digits.ToArray())), currentPacket));
                }
            }

            throw new ArgumentException("Malformed packet.");
        }

        private abstract class PacketItem : IComparable<PacketInteger>, IComparable<Packet>
        {
            public Packet? Parent;

            public PacketItem(Packet? parent)
            {
                Parent = parent;
            }

            public abstract int CompareTo(PacketInteger? other);

            public abstract int CompareTo(Packet? other);
        }

        private class PacketInteger : PacketItem
        {
            public int Value { get; init; }

            public PacketInteger(int value, Packet? parent) : base(parent)
            {
                Value = value;
            }

            public override int CompareTo(PacketInteger? other)
            {
                if (other == null)
                    return -1;

                return Value.CompareTo(other.Value);
            }

            public override int CompareTo(Packet? other)
            {
                if (other == null)
                    return -1;

                Packet list = new(null);
                list.AddItem(this);

                return list.CompareTo(other);
            }
        }

        private class Packet : PacketItem, IComparable<PacketInteger>, IComparable<Packet>
        {
            public List<PacketItem> Items { get; private set; }

            public Packet(Packet? parent) : base(parent)
            {
                Items = new List<PacketItem>();
            }

            public void AddItem(PacketItem item)
            {
                Items.Add(item);
            }

            public override int CompareTo(PacketInteger? other)
            {
                if (other == null)
                    return -1;

                Packet list = new(null);
                list.AddItem(other);

                return CompareTo(list);
            }

            public override int CompareTo(Packet? other)
            {
                if (other == null)
                    return -1;

                for (int i = 0; i < Math.Min(Items.Count, other.Items.Count); i++)
                {
                    int compareItems;

                    if (other.Items[i].GetType() == typeof(PacketInteger))
                        compareItems = Items[i].CompareTo((PacketInteger)other.Items[i]);
                    else
                        compareItems = Items[i].CompareTo((Packet)other.Items[i]);

                    if (compareItems != 0)
                        return compareItems;
                }

                return Items.Count.CompareTo(other.Items.Count);
            }
        }
    }
}
