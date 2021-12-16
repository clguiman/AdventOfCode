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
            Assert.Equal(6, Solve("D2FE28").Version);
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(16, Solve("8A004A801A8002F478").Version);
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(12, Solve("620080001611562C8802118E34").Version);
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(23, Solve("C0015000016115A2E0802F182340").Version);
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(31, Solve("A0016C880162017C3686B18A3D4780").Version);
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(979, Solve(File.ReadAllText("input/day16.txt")).Version);
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal(3, Solve("C200B40A82").Value);
        }

        [Fact]
        public void Test8()
        {
            Assert.Equal(54, Solve("04005AC33890").Value);
        }

        [Fact]
        public void Test9()
        {
            Assert.Equal(7, Solve("880086C3E88112").Value);
        }

        [Fact]
        public void Test10()
        {
            Assert.Equal(9, Solve("CE00C43D881120").Value);
        }

        [Fact]
        public void Test11()
        {
            Assert.Equal(1, Solve("D8005AC2A8F0").Value);
        }

        [Fact]
        public void Test12()
        {
            Assert.Equal(0, Solve("F600BC2D8F").Value);
        }

        [Fact]
        public void Test13()
        {
            Assert.Equal(0, Solve("9C005AC2F8F0").Value);
        }

        [Fact]
        public void Test14()
        {
            Assert.Equal(1, Solve("9C0141080250320F1802104A08").Value);
        }

        [Fact]
        public void Test15()
        {
            Assert.Equal(277110354175, Solve(File.ReadAllText("input/day16.txt")).Value);
        }

        private static LiteralPacket Solve(string input)
        {
            var bits = input.SelectMany(hexDigit =>
                {
                    var n = Convert.ToUInt16(string.Concat(hexDigit), 16);
                    return new[] { (ushort)((n & 0x8) >> 3), (ushort)((n & 0x4) >> 2), (ushort)((n & 0x2) >> 1), (ushort)(n & 0x1) };
                }).ToArray();

            (_, _, var root) = Packet.ReadPacket(bits, bits.Length);
            return root.Solve();
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
                if (maxLength < 11)
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

            public LiteralPacket Solve()
            {
                var root = this;
                while (root.Type != PacketType.Literal)
                {
                    root = ResolveLeafOperations(root);
                }

                return root as LiteralPacket;
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

                if (hasLengthInBits)
                {
                    var subPacketsLength = NumberFromBits(curBits.Take(15));
                    curBits = curBits.Skip(15);
                    bitsRead += 15;

                    List<Packet> subPackets = new();
                    do
                    {
                        (curBits, var subPacketSize, var subPacket) = ReadPacket(curBits, subPacketsLength);
                        subPacketsLength -= subPacketSize;
                        bitsRead += subPacketSize;
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
                        subPackets.Add(subPacket);
                        bitsRead += subPacketSize;
                    }

                    return (curBits, bitsRead, new OperatorPacket(ver, type, subPackets));
                }
            }

            public LiteralPacket ComputeOperation()
            {
                var ver = Version + SubPackets.Sum(s => s.Version);
                switch (Type)
                {
                    case PacketType.Sum:
                        return new(ver, SubPackets.Select(s => (s as LiteralPacket).Value).Sum());
                    case PacketType.Product:
                        return new(ver, SubPackets.Select(s => (s as LiteralPacket).Value).Aggregate((a, b) => a * b));
                    case PacketType.Min:
                        return new(ver, SubPackets.Select(s => (s as LiteralPacket).Value).Min());
                    case PacketType.Max:
                        return new(ver, SubPackets.Select(s => (s as LiteralPacket).Value).Max());
                    case PacketType.Gt:
                    case PacketType.Lt:
                    case PacketType.Eq:
                        {
                            var a = (SubPackets[0] as LiteralPacket).Value;
                            var b = (SubPackets[1] as LiteralPacket).Value;
                            if (Type == PacketType.Gt)
                            {
                                return new(ver, a > b ? 1 : 0);
                            }
                            else if (Type == PacketType.Lt)
                            {
                                return new(ver, a < b ? 1 : 0);
                            }
                            else
                            {
                                return new(ver, a == b ? 1 : 0);
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

            public LiteralPacket(int version, long value) : base(version, PacketType.Literal)
            {
                Value = value;
            }
        }
    }
}
