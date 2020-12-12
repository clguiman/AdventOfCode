using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2020
{
    public class Day12
    {
        [Fact]
        public void Test1()
        {
            var input = new[] {
                "F10",
                "N3",
                "F7",
                "R90",
                "F11"};
            Assert.Equal(25, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(415, Part1(File.ReadAllLines("input/day12.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input = new[] {
                "F10",
                "N3",
                "F7",
                "R90",
                "F11"};
            Assert.Equal(286, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(29401, Part2(File.ReadAllLines("input/day12.txt")));
        }
        private static long Part1(IEnumerable<string> input) => Solution(input, false);
        private static long Part2(IEnumerable<string> input) => Solution(input, true);
        private static long Solution(IEnumerable<string> input, bool useWayPoint)
        {
            var curPos = (X: 0L, Y: 0L);
            var direction = useWayPoint ? (X: -10L, Y: 1L) : (X: -1L, Y: 0L); // east

            foreach (var instr in input
                .Select(x => new Instruction()
                {
                    Action = ParseAction(x[0]),
                    Value = long.Parse(x.Substring(1))
                }))
            {
                switch (instr.Action)
                {
                    case ShipAction.North:
                        if (useWayPoint)
                        {
                            direction = (direction.X, direction.Y + instr.Value);
                        }
                        else
                        {
                            curPos = (curPos.X, curPos.Y + instr.Value);
                        }
                        break;
                    case ShipAction.South:
                        if (useWayPoint)
                        {
                            direction = (direction.X, direction.Y - +instr.Value);
                        }
                        else
                        {
                            curPos = (curPos.X, curPos.Y - instr.Value);
                        }
                        break;
                    case ShipAction.East:
                        if (useWayPoint)
                        {
                            direction = (direction.X - instr.Value, direction.Y);
                        }
                        else
                        {
                            curPos = (curPos.X - instr.Value, curPos.Y);
                        }
                        break;
                    case ShipAction.West:
                        if (useWayPoint)
                        {
                            direction = (direction.X + instr.Value, direction.Y);
                        }
                        else
                        {
                            curPos = (curPos.X + instr.Value, curPos.Y);
                        }
                        break;
                    case ShipAction.Left:
                        for (var i = 0; i < instr.Value; i += 90)
                        {
                            direction = RotateBy90(direction, right: false);
                        }
                        break;
                    case ShipAction.Right:
                        for (var i = 0; i < instr.Value; i += 90)
                        {
                            direction = RotateBy90(direction, right: true);
                        }
                        break;
                    case ShipAction.Forward:
                        curPos = (curPos.X + instr.Value * direction.X, curPos.Y + instr.Value * direction.Y);
                        break;
                    default:
                        throw new Exception("invalid action!");
                }
            }

            return Math.Abs(curPos.X) + Math.Abs(curPos.Y);
        }

        private static (long X, long Y) RotateBy90((long X, long Y) direction, bool right) => right switch
        {
            true => (-1 * direction.Y, direction.X),
            false => (direction.Y, -1 * direction.X),
        };
        private static ShipAction ParseAction(char a) => a switch
        {
            'N' => ShipAction.North,
            'S' => ShipAction.South,
            'E' => ShipAction.East,
            'W' => ShipAction.West,
            'L' => ShipAction.Left,
            'R' => ShipAction.Right,
            'F' => ShipAction.Forward,
            _ => throw new ArgumentException("invalid action!"),
        };
        enum ShipAction
        {
            North,
            South,
            East,
            West,
            Left,
            Right,
            Forward,
        }

        struct Instruction
        {
            public ShipAction Action { get; init; }
            public long Value { get; init; }
        }
    }
}
