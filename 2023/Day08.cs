namespace _2023
{
    public class Day08
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, SolvePart1(ParseInput([
            "RL",
            "",
            "AAA = (BBB, CCC)",
            "BBB = (DDD, EEE)",
            "CCC = (ZZZ, GGG)",
            "DDD = (DDD, DDD)",
            "EEE = (EEE, EEE)",
            "GGG = (GGG, GGG)",
            "ZZZ = (ZZZ, ZZZ)"
                ])));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(12737, SolvePart1(ParseInput(File.ReadAllLines("input/day08.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(6, SolvePart2(ParseInput([
            "LR",
            "",
            "11A = (11B, XXX)",
            "11B = (XXX, 11Z)",
            "11Z = (11B, XXX)",
            "22A = (22B, XXX)",
            "22B = (22C, 22C)",
            "22C = (22Z, 22Z)",
            "22Z = (22B, 22B)",
            "XXX = (XXX, XXX)"
                ])));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(9064949303801L, SolvePart2(ParseInput(File.ReadAllLines("input/day08.txt"))));
        }

        private static int SolvePart1((Instructions instructions, IEnumerable<Node> nodes) input)
        {
            var curNode = input.nodes.First(n => n.Name.Equals("AAA", StringComparison.Ordinal));
            var instructions = input.instructions;
            int steps = 0;
            while (!curNode.Name.Equals("ZZZ", StringComparison.Ordinal))
            {
                if (instructions.Next() == Instruction.Left)
                {
                    curNode = curNode.Left;
                }
                else
                {
                    curNode = curNode.Right;
                }
                steps++;
            }
            return steps;
        }

        private static long SolvePart2((Instructions instructions, IEnumerable<Node> nodes) input)
        {
            var nodes = input.nodes
                                .Where(n => n.Name.EndsWith('A'))
                                .Select(n => (curNode: n, cycleSize: 0, cycleFound: false))
                                .ToArray();

            var instructions = input.instructions;
            while (nodes.Any(n => !n.cycleFound))
            {
                var instruction = instructions.Next();
                for (var idx = 0; idx < nodes.Length; idx++)
                {
                    if (!nodes[idx].cycleFound)
                    {
                        if (instruction == Instruction.Left)
                        {
                            nodes[idx].curNode = nodes[idx].curNode.Left;
                        }
                        else
                        {
                            nodes[idx].curNode = nodes[idx].curNode.Right;
                        }

                        if (nodes[idx].curNode.Name.EndsWith('Z'))
                        {
                            nodes[idx].cycleFound = true;
                        }
                        nodes[idx].cycleSize++;
                    }
                }
            }

            return nodes.Select(n => (long)n.cycleSize).Aggregate(Arithmetic.LeastCommonMultiple);
        }

        private static (Instructions instructions, IEnumerable<Node> nodes) ParseInput(IEnumerable<string> input)
        {
            var instructions = new Instructions(input.First());
            Dictionary<string, Node> nodeDict = [];

            foreach (var nodeStr in input
                                        .Skip(2)
                                        .Select(line =>
                                                {
                                                    var t = line.Split("=").Select(x => x.Trim()).ToArray();
                                                    var origin = t[0];
                                                    var t2 = t[1].Split(",").Select(x => x.Trim()).ToArray();
                                                    var left = t2[0][1..];
                                                    var right = t2[1][..^1];
                                                    return (origin, left, right);
                                                }))
            {
                if (!nodeDict.TryGetValue(nodeStr.origin, out var origin))
                {
                    origin = new Node { Name = nodeStr.origin };
                    nodeDict.Add(nodeStr.origin, origin);
                }

                if (!nodeDict.TryGetValue(nodeStr.left, out var left))
                {
                    left = new Node { Name = nodeStr.left };
                    nodeDict.Add(nodeStr.left, left);
                }

                if (!nodeDict.TryGetValue(nodeStr.right, out var right))
                {
                    right = new Node { Name = nodeStr.right };
                    nodeDict.Add(nodeStr.right, right);
                }

                origin.Left = left;
                origin.Right = right;
            }

            return (instructions, nodeDict.Values.ToArray());
        }

        private class Instructions(string instructionsStr)
        {
            public Instruction Next()
            {
                Instruction ret = _instructions[_curPos];
                if (_curPos < _instructions.Length - 1)
                {
                    _curPos++;
                }
                else
                {
                    _curPos = 0;
                }

                return ret;
            }

            private int _curPos = 0;
            private readonly Instruction[] _instructions = instructionsStr.Select(c => c switch
                {
                    'L' => Instruction.Left,
                    'R' => Instruction.Right,
                    _ => throw new Exception()
                }).ToArray();
        }

        public class Node
        {
            public string Name { get; init; }

            public Node Left { get; set; }

            public Node Right { get; set; }
        }

        private enum Instruction
        {
            Left,
            Right
        }
    }
}
