using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2022
{
    public class Day11
    {
        private const string testData =
            "Monkey 0:\r\n" +
            "  Starting items: 79, 98\r\n" +
            "  Operation: new = old * 19\r\n" +
            "  Test: divisible by 23\r\n" +
            "    If true: throw to monkey 2\r\n" +
            "    If false: throw to monkey 3\r\n" +
            "\r\n" +
            "Monkey 1:\r\n" +
            "  Starting items: 54, 65, 75, 74\r\n" +
            "  Operation: new = old + 6\r\n" +
            "  Test: divisible by 19\r\n" +
            "    If true: throw to monkey 2\r\n" +
            "    If false: throw to monkey 0\r\n" +
            "\r\n" +
            "Monkey 2:\r\n" +
            "  Starting items: 79, 60, 97\r\n" +
            "  Operation: new = old * old\r\n" +
            "  Test: divisible by 13\r\n" +
            "    If true: throw to monkey 1\n" +
            "    If false: throw to monkey 3\n" +
            "\n" +
            "Monkey 3:\r\n" +
            "  Starting items: 74\n" +
            "  Operation: new = old + 3\n" +
            "  Test: divisible by 17\n" +
            "    If true: throw to monkey 0\r\n" +
            "    If false: throw to monkey 1";

        [Fact]
        public void Test1()
        {
            Assert.Equal(10605, Solve(ParseInput(testData.Split('\n')), 20, 3));
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal(56120, Solve(ParseInput(File.ReadAllLines("input/day11.txt")), 20, 3));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(2713310158, Solve(ParseInput(testData.Split('\n')), 10_000, 1));
        }
        [Fact]
        public void Test4()
        {
            Assert.Equal(24389045529, Solve(ParseInput(File.ReadAllLines("input/day11.txt")), 10_000, 1));
        }

        private static long Solve(List<Monkey> monkeys, int rounds, int worryDivisor)
        {
            // they're all prime numbers, the lowest common multiple will be their product.
            var lcm = monkeys.Select(x => x.DivisibleTestValue).Distinct().Aggregate((x, y) => x * y);
            for (var i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    var initialItemCount = monkey.WorryLevels.Count;
                    monkey.InspectionCount += initialItemCount;
                    for (var c = 0; c < initialItemCount; c++)
                    {
                        int item = monkey.WorryLevels.Dequeue();
                        item = (int)(monkey.OperationFunc((long)item) % (long)lcm);
                        item /= worryDivisor;

                        if (item % monkey.DivisibleTestValue == 0)
                        {
                            monkeys[monkey.TestResultTrueDestination].WorryLevels.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.TestResultFalseDestination].WorryLevels.Enqueue(item);
                        }
                    }
                }
            }
            return monkeys.Select(x => x.InspectionCount).OrderDescending().Take(2).Select(x => (long)x).Aggregate((x, y) => x * y);
        }

        private static List<Monkey> ParseInput(IEnumerable<string> input)
        {
            List<Monkey> monkeys = new();

            foreach (var line in input.Select(x => x.Trim()))
            {
                if (line.StartsWith("Monkey", StringComparison.OrdinalIgnoreCase))
                {
                    monkeys.Add(new Monkey());
                    continue;
                }
                if (line.StartsWith("Starting items:", StringComparison.OrdinalIgnoreCase))
                {
                    monkeys[^1].WorryLevels = new(line.Split(':')[1].Split(',').Select(int.Parse));
                    continue;
                }
                if (line.StartsWith("Test: divisible by", StringComparison.OrdinalIgnoreCase))
                {
                    monkeys[^1].DivisibleTestValue = int.Parse(line.Split(' ')[^1]);
                    continue;
                }
                if (line.StartsWith("If true:", StringComparison.OrdinalIgnoreCase))
                {
                    monkeys[^1].TestResultTrueDestination = int.Parse(line.Split(' ')[^1]);
                    continue;
                }
                if (line.StartsWith("If false:", StringComparison.OrdinalIgnoreCase))
                {
                    monkeys[^1].TestResultFalseDestination = int.Parse(line.Split(' ')[^1]);
                    continue;
                }
                if (line.StartsWith("Operation: new =", StringComparison.OrdinalIgnoreCase))
                {
                    var op = line.Split('=')[1].Trim()[3..].Trim();
                    var isAddition = op[0] == '+';
                    if (op.EndsWith("old", StringComparison.OrdinalIgnoreCase))
                    {
                        monkeys[^1].OperationFunc = x => isAddition ? x + x : x * x;
                    }
                    else
                    {
                        var num = int.Parse(op.Split(' ')[1]);
                        monkeys[^1].OperationFunc = x => isAddition ? x + num : x * num;
                    }
                    continue;
                }
            }

            return monkeys;
        }

        private record Monkey
        {
            public Queue<int> WorryLevels { get; set; }
            public Func<long, long> OperationFunc { get; set; }
            public int DivisibleTestValue { get; set; }
            public int TestResultTrueDestination { get; set; }
            public int TestResultFalseDestination { get; set; }
            public int InspectionCount { get; set; }
        }
    }
}