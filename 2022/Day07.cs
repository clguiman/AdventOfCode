using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2022
{
    public class Day07
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(95437, Part1(BuildFileSystem(new[] {
                "$ cd /",
                "$ ls",
                "dir a",
                "14848514 b.txt",
                "8504156 c.dat",
                "dir d",
                "$ cd a",
                "$ ls",
                "dir e",
                "29116 f",
                "2557 g",
                "62596 h.lst",
                "$ cd e",
                "$ ls",
                "584 i",
                "$ cd ..",
                "$ cd ..",
                "$ cd d",
                "$ ls",
                "4060174 j",
                "8033020 d.log",
                "5626152 d.ext",
                "7214296 k"
            })));
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal(1770595, Part1(BuildFileSystem(File.ReadAllLines("input/day07.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(24933642, Part2(BuildFileSystem(new[] {
                "$ cd /",
                "$ ls",
                "dir a",
                "14848514 b.txt",
                "8504156 c.dat",
                "dir d",
                "$ cd a",
                "$ ls",
                "dir e",
                "29116 f",
                "2557 g",
                "62596 h.lst",
                "$ cd e",
                "$ ls",
                "584 i",
                "$ cd ..",
                "$ cd ..",
                "$ cd d",
                "$ ls",
                "4060174 j",
                "8033020 d.log",
                "5626152 d.ext",
                "7214296 k"
            })));
        }
        [Fact]
        public void Test4()
        {
            Assert.Equal(2195372, Part2(BuildFileSystem(File.ReadAllLines("input/day07.txt"))));
        }

        private static int Part1(FsNode fsRoot)
        {
            var sum = 0;
            fsRoot.Walk((node) =>
            {
                if (node.Type == FsNode.NodeType.Dir && node.Size <= 100_000)
                {
                    sum += node.Size;
                }
            });
            return sum;
        }

        private static int Part2(FsNode fsRoot)
        {
            var neededSize = 30_000_000 - (70_000_000 - fsRoot.Size);
            List<int> candidateSizes = new();
            fsRoot.Walk((node) =>
            {
                if (node.Type == FsNode.NodeType.Dir && node.Size >= neededSize)
                {
                    candidateSizes.Add(node.Size);
                }
            });
            return candidateSizes.Order().First();
        }

        private static FsNode BuildFileSystem(IEnumerable<string> input)
        {
            FsNode root = new() { Name = "/", Type = FsNode.NodeType.Dir, Size = 0, Children = new(), Parent = null };
            FsNode curNode = root;

            foreach (var line in input)
            {
                var tokens = line.Split(' ');
                switch (tokens[0])
                {

                    case "$":
                        {
                            switch (tokens[1])
                            {
                                case "ls":
                                    {
                                        break;
                                    }
                                case "cd":
                                    {
                                        if (string.Equals("/", tokens[2], StringComparison.Ordinal))
                                        {
                                            curNode = root;
                                            break;
                                        }
                                        if (string.Equals("..", tokens[2], StringComparison.Ordinal))
                                        {
                                            curNode = curNode.Parent;
                                            break;
                                        }
                                        curNode = curNode.Children.FirstOrDefault(x => string.Equals(x.Name, tokens[2], StringComparison.Ordinal));
                                        break;
                                    }
                                default:
                                    throw new ArgumentException(line);
                            }
                        }
                        break;
                    default:
                        {
                            var name = tokens[1].Trim();
                            FsNode.NodeType type = FsNode.NodeType.File;
                            int size = 0;
                            if (string.Equals("dir", tokens[0], StringComparison.Ordinal))
                            {
                                type = FsNode.NodeType.Dir;
                            }
                            else
                            {
                                size = int.Parse(tokens[0]);
                            }
                            FsNode newNode = new() { Name = name, Type = type, Size = size, Children = new(), Parent = curNode };
                            curNode.Children.Add(newNode);
                            break;
                        }
                }
            }
            root.ComputeSize();
            return root;
        }

        private record FsNode
        {
            public enum NodeType
            {
                Dir,
                File
            }

            public NodeType Type { get; init; }
            public string Name { get; init; }
            public int Size { get; set; }
            public FsNode Parent { get; init; }
            public List<FsNode> Children { get; init; }

            public int ComputeSize()
            {
                foreach (var item in Children)
                {
                    Size += item.ComputeSize();
                }
                return Size;
            }

            public void Walk(Action<FsNode> observer)
            {
                observer(this);
                foreach (var item in Children)
                {
                    item.Walk(observer);
                }
            }
        }
    }
}