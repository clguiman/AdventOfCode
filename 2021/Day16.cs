using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day16
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(6, Part1("D2FE28"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(16, Part1("8A004A801A8002F478"));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(12, Part1("620080001611562C8802118E34"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(23, Part1("C0015000016115A2E0802F182340"));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(31, Part1("A0016C880162017C3686B18A3D4780"));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(979, Part1(File.ReadAllText("input/day16.txt")));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal(3, Part2("C200B40A82"));
        }

        [Fact]
        public void Test8()
        {
            Assert.Equal(54, Part2("04005AC33890"));
        }

        [Fact]
        public void Test9()
        {
            Assert.Equal(7, Part2("880086C3E88112"));
        }

        [Fact]
        public void Test10()
        {
            Assert.Equal(9, Part2("CE00C43D881120"));
        }

        [Fact]
        public void Test11()
        {
            Assert.Equal(1, Part2("D8005AC2A8F0"));
        }

        [Fact]
        public void Test12()
        {
            Assert.Equal(0, Part2("F600BC2D8F"));
        }

        [Fact]
        public void Test13()
        {
            Assert.Equal(0, Part2("9C005AC2F8F0"));
        }

        [Fact]
        public void Test14()
        {
            Assert.Equal(1, Part2("9C0141080250320F1802104A08"));
        }

        [Fact]
        public void Test15()
        {
            Assert.Equal(277110354175, Part2(File.ReadAllText("input/day16.txt")));
        }

        private static int Part1(string input)
        {
            var sum = 0;
            var bits = input.SelectMany(BitsFromHexDigit).ToArray();

            int availableData = bits.Length;
            IEnumerable<ushort> curBits = bits;
            bool isMoreDataAvailable;
            do
            {
                (curBits, var bitsRead, var packet) = Packet.ReadPacket(bits, availableData);
                availableData -= bitsRead;
                sum += GetVersionSum(packet);

                isMoreDataAvailable = (availableData > 6);
            } while (isMoreDataAvailable);

            return sum;
        }
        private static int GetVersionSum(Packet p)
        {
            if (p == null)
            {
                return 0;
            }
            if (p is OperatorPacket opPacket)
            {
                return p.Version + opPacket.SubPackets.Select(GetVersionSum).Sum();
            }

            return p.Version;
        }

        private static long Part2(string input)
        {
            var bits = input.SelectMany(BitsFromHexDigit).ToArray();

            (_, _, var root) = Packet.ReadPacket(bits, bits.Length);
            while (root.Type != Packet.PacketType.Literal)
            {
                root = ResolveLeafOperations(root);
            }

            return (root as LiteralPacket).Value;
        }

        private static Packet ResolveLeafOperations(Packet root)
        {
            if (root is OperatorPacket opPacket)
            {
                if (opPacket.SubPackets.All(s => s is LiteralPacket))
                {
                    return opPacket.ComputeOperation();
                }
                opPacket.ReplaceSubPackets(opPacket.SubPackets.Select(ResolveLeafOperations).ToList());
                return opPacket;
            }
            return root;
        }

        private static IEnumerable<ushort> BitsFromHexDigit(char hexDigit)
        {
            var n = Convert.ToUInt16(string.Concat(hexDigit), 16);
            var ret = new ushort[4];
            for (var i = 3; i >= 0; i--)
            {
                ret[i] = (ushort)(n & 0x1);
                n >>= 1;
            }
            return ret;
        }

        private class Packet
        {
            public enum PacketType
            {
                Sum = 0,
                Product = 1,
                Min = 2,
                Max = 3,
                Literal = 4,
                Gt = 5,
                Lt = 6,
                Eq = 7,
            }

            public int Version { get; }
            public PacketType Type { get; }

            public static (IEnumerable<ushort> leftoverSequence, int bitsRead, Packet packet) ReadPacket(IEnumerable<ushort> bits, int maxLength)
            {
                if (maxLength < 6)
                {
                    return (Enumerable.Empty<ushort>(), maxLength, null);
                }

                IEnumerable<ushort> curBits = bits;
                var type = NumberFromBits(curBits.Skip(3).Take(3));

                Packet packet;
                int bitsRead;
                if (type == 4) // literal
                {
                    (curBits, bitsRead, packet) = LiteralPacket.ReadLiteralPacket(curBits);
                }
                else
                {
                    (curBits, bitsRead, packet) = OperatorPacket.ReadOperatorPacket(curBits, maxLength);
                }

                return (curBits, bitsRead, packet);
            }

            protected static int NumberFromBits(IEnumerable<ushort> bits)
            {
                int ret = 0;
                foreach (var bit in bits)
                {
                    ret += bit;
                    ret *= 2;
                }
                return ret / 2;
            }

            protected Packet(int version, PacketType type)
            {
                Version = version;
                Type = type;
            }
        }

        private class OperatorPacket : Packet
        {
            public List<Packet> SubPackets { get; private set; }

            public static (IEnumerable<ushort> leftoverSequence, int bitsRead, OperatorPacket packet) ReadOperatorPacket(IEnumerable<ushort> bits, int maxLength)
            {
                int bitsRead = 0;
                IEnumerable<ushort> curBits = bits;
                var ver = NumberFromBits(curBits.Take(3));
                var type = NumberFromBits(curBits.Skip(3).Take(3));
                curBits = curBits.Skip(6);
                bitsRead += 6;

                if (type == 4)
                {
                    throw new Exception("Not an operator packet!");
                }

                bool hasLengthInBits = (curBits.First() == 0);
                curBits = curBits.Skip(1);
                bitsRead += 1;

                if (bitsRead >= maxLength)
                {
                    return (Enumerable.Empty<ushort>(), maxLength, null);
                }

                if (hasLengthInBits)
                {
                    var subPacketsLength = NumberFromBits(curBits.Take(15));
                    curBits = curBits.Skip(15);
                    bitsRead += 15;

                    List<Packet> subPackets = new();
                    do
                    {
                        (curBits, var subPacketSize, var subPacket) = Packet.ReadPacket(curBits, subPacketsLength);
                        subPacketsLength -= subPacketSize;
                        bitsRead += subPacketSize;
                        if (subPacket == null)
                        {
                            return (Enumerable.Empty<ushort>(), maxLength, null);
                        }

                        subPackets.Add(subPacket);

                    } while (subPacketsLength > 0);

                    return (curBits, bitsRead, new OperatorPacket(ver, type, subPackets));
                }
                else
                {
                    var subPacketCount = NumberFromBits(curBits.Take(11));
                    curBits = curBits.Skip(11);
                    bitsRead += 11;

                    List<Packet> subPackets = new();
                    for (var idx = 0; idx < subPacketCount; idx++)
                    {
                        (curBits, var subPacketSize, var subPacket) = ReadPacket(curBits, maxLength - bitsRead);
                        if (subPacket == null)
                        {
                            return (Enumerable.Empty<ushort>(), maxLength, null);
                        }

                        subPackets.Add(subPacket);
                        bitsRead += subPacketSize;
                    }

                    return (curBits, bitsRead, new OperatorPacket(ver, type, subPackets));
                }
            }

            public LiteralPacket ComputeOperation()
            {
                switch (Type)
                {
                    case PacketType.Sum:
                        return LiteralPacket.FromNumber(SubPackets.Select(s => (s as LiteralPacket).Value).Sum());
                    case PacketType.Product:
                        return LiteralPacket.FromNumber(SubPackets.Select(s => (s as LiteralPacket).Value).Aggregate((a, b) => a * b));
                    case PacketType.Min:
                        return LiteralPacket.FromNumber(SubPackets.Select(s => (s as LiteralPacket).Value).Min());
                    case PacketType.Max:
                        return LiteralPacket.FromNumber(SubPackets.Select(s => (s as LiteralPacket).Value).Max());
                    case PacketType.Gt:
                    case PacketType.Lt:
                    case PacketType.Eq:
                        {
                            var a = (SubPackets[0] as LiteralPacket).Value;
                            var b = (SubPackets[1] as LiteralPacket).Value;
                            if (Type == PacketType.Gt)
                            {
                                return LiteralPacket.FromNumber(a > b ? 1 : 0);
                            }
                            else if (Type == PacketType.Lt)
                            {
                                return LiteralPacket.FromNumber(a < b ? 1 : 0);
                            }
                            else
                            {
                                return LiteralPacket.FromNumber(a == b ? 1 : 0);
                            }

                        }
                    default: throw new InvalidOperationException();
                }
            }

            public void ReplaceSubPackets(List<Packet> newPackets)
            {
                SubPackets = newPackets;
            }

            private OperatorPacket(int version, int type, List<Packet> subPackets) : base(version, (PacketType)type)
            {
                SubPackets = subPackets;
            }
        }

        private class LiteralPacket : Packet
        {
            public long Value { get; }

            public static LiteralPacket FromNumber(long n) => new(0, n);

            public static (IEnumerable<ushort> leftoverSequence, int bitsRead, LiteralPacket packet) ReadLiteralPacket(IEnumerable<ushort> bits)
            {
                int bitsRead = 0;
                IEnumerable<ushort> curBits = bits;
                var ver = NumberFromBits(curBits.Take(3));
                var type = NumberFromBits(curBits.Skip(3).Take(3));
                curBits = curBits.Skip(6);
                bitsRead += 6;

                if (type != 4)
                {
                    throw new Exception("Not a literal packet!");
                }

                long value = 0;
                bool endOfSequence;
                do
                {
                    endOfSequence = (curBits.First() == 0);
                    curBits = curBits.Skip(1);

                    value += (long)NumberFromBits(curBits.Take(4));
                    value <<= 4;

                    curBits = curBits.Skip(4);

                    bitsRead += 5;
                } while (!endOfSequence);

                return (curBits, bitsRead, new LiteralPacket(ver, value >> 4));
            }

            private LiteralPacket(int version, long value) : base(version, PacketType.Literal)
            {
                Value = value;
            }
        }
    }
}
