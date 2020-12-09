using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day06
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(42, Solution1(new[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L" }));
            Assert.Equal(6, Solution1(new[] { "A)B", "B)C", "C)D" }));
            Assert.Equal(6, Solution1(new[] { "C)D", "B)C", "A)B" }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(135690, Solution1(File.ReadAllLines("input/day6.txt")));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(4, Solution2(new[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN" }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(298, Solution2(File.ReadAllLines("input/day6.txt")));
        }

        private static int Solution1(IEnumerable<string> input)
        {
            Dictionary<string, Node> objects = new();

            foreach (var item in input)
            {
                var tokens = item.Split(')');
                if (!objects.TryGetValue(tokens[0], out var parent))
                {
                    parent = new Node() { Name = tokens[0], Parent = null, OrbitCount = 0, ChildrenCount = 0, Children = new List<Node>() };
                    objects.Add(parent.Name, parent);
                }
                if (!objects.TryGetValue(tokens[1], out var child))
                {
                    child = new Node() { Name = tokens[1], Parent = parent, OrbitCount = 0, ChildrenCount = 0, Children = new List<Node>() };
                    objects.Add(child.Name, child);
                }
                else
                {
                    if (child.Parent != null)
                    {
                        throw new Exception("Invalid input");
                    }
                    child.Parent = parent;
                }

                parent.Children.Add(child);

                // update all nodes upstream;
                var cur = child;
                Node prev = Node.IsolatedNode;
                while (cur.Parent != null)
                {
                    cur.Parent.ChildrenCount = cur.Parent.Children.Sum(n => n.ChildrenCount) + cur.Parent.Children.Count;
                    var otherChildrensOrbitCount = cur.Parent.Children.Where(c => c != cur).Sum(n => n.OrbitCount);
                    cur.Parent.OrbitCount = cur.Parent.ChildrenCount - cur.ChildrenCount - 1 + otherChildrensOrbitCount + (cur.ChildrenCount + cur.OrbitCount + 1);
                    prev = cur;
                    cur = cur.Parent;
                }
            }
            return objects.Where(x => x.Value.Parent == null).Select(x => x.Value.OrbitCount).Sum();
        }

        private static int Solution2(IEnumerable<string> input)
        {
            Dictionary<string, Node2> objects = new();

            foreach (var item in input)
            {
                var tokens = item.Split(')');
                if (!objects.TryGetValue(tokens[0], out var parent))
                {
                    parent = new Node2() { Name = tokens[0], Parent = null, DistanceToSanta = -1 };
                    objects.Add(parent.Name, parent);
                }
                if (!objects.TryGetValue(tokens[1], out var child))
                {
                    child = new Node2() { Name = tokens[1], Parent = parent, DistanceToSanta = -1 };
                    objects.Add(child.Name, child);
                }
                else
                {
                    if (child.Parent != null)
                    {
                        throw new Exception("Invalid input");
                    }
                    child.Parent = parent;
                }
            }

            var santa = objects["SAN"];
            var cur = santa;
            var distance = 0;
            while (cur.Parent != null)
            {
                cur.DistanceToSanta = distance;
                distance++;
                cur = cur.Parent;
            }

            var you = objects["YOU"];
            cur = you;
            distance = 0;
            while (cur.Parent != null)
            {
                if (cur.DistanceToSanta != -1)
                {
                    return distance + cur.DistanceToSanta - 2;
                }
                distance++;
                cur = cur.Parent;
            }

            return -1;
        }

        private class Node
        {
            public string Name { get; init; }
            public Node Parent { get; set; }
            public List<Node> Children { get; init; }
            public int OrbitCount { get; set; }
            public int ChildrenCount { get; set; }

            public static readonly Node IsolatedNode = new() { Name = "#INVALID#", Parent = null, OrbitCount = 0, ChildrenCount = 0, Children = new List<Node>() };
        }

        private class Node2
        {
            public string Name { get; init; }
            public Node2 Parent { get; set; }
            public int DistanceToSanta { get; set; }
        }
    }
}
