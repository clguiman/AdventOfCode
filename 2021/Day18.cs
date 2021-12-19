using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2021
{
    public class Day18
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(29, Part1(ParseInput(new[] { "[9, 1]" })));
            Assert.Equal(3488, Part1(ParseInput(new[] { "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]" })));
            Assert.Equal(548, Part1(ParseInput(new[] { "[[[[[9,8],1],2],3],4]" })));
            Assert.Equal(285, Part1(ParseInput(new[] { "[7,[6,[5,[4,[3,2]]]]]" })));
            Assert.Equal(402, Part1(ParseInput(new[] { "[[6,[5,[4,[3,2]]]],1]" })));
            Assert.Equal(633, Part1(ParseInput(new[] { "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]" })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(3488, Part1(ParseInput(new[] {
                "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
                "[7,[[[3,7],[4,3]],[[6, 3],[8, 8]]]]",
                "[[2,[[0, 8],[3, 4]]],[[[6,7],1],[7,[1,6]]]]",
                "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
                "[7,[5,[[3,8],[1,4]]]]",
                "[[2,[2,2]],[8,[8,1]]]",
                "[2,9]",
                "[1,[[[9,3],9],[[9,0],[0,7]]]]",
                "[[[5,[7,4]],7],1]",
                "[[[[4,2],2],6],[8,7]]",
            })));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(4140, Part1(ParseInput(new[] {
                "[[[0,[5, 8]],[[1, 7],[9, 6]]],[[4,[1, 2]],[[1, 4], 2]]]",
                "[[[5,[2,8]],4],[5,[[9,9],0]]]",
                "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
                "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
                "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
                "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
                "[[[[5,4],[7,7]],8],[[8,3],8]]",
                "[[9,3],[[9,9],[6,[4,9]]]]",
                "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
                "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"
            })));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(3216, Part1(ParseInput(File.ReadAllLines("input/day18.txt"))));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(3993, Part2(ParseInput(new[] {
                "[[[0,[5, 8]],[[1, 7],[9, 6]]],[[4,[1, 2]],[[1, 4], 2]]]",
                "[[[5,[2,8]],4],[5,[[9,9],0]]]",
                "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
                "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
                "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
                "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
                "[[[[5,4],[7,7]],8],[[8,3],8]]",
                "[[9,3],[[9,9],[6,[4,9]]]]",
                "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
                "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"
            })));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal(4643, Part2(ParseInput(File.ReadAllLines("input/day18.txt"))));
        }

        private static long Part1(IEnumerable<TreeNode> input)
        {
            var numbers = input.ToArray();
            var sum = numbers[0].Clone();
            for (var idx = 1; idx < numbers.Length; idx++)
            {
                sum = sum.Add(numbers[idx]);
                sum.Reduce();
            }
            sum.Reduce();

            return sum.Magnitude;
        }

        private static long Part2(IEnumerable<TreeNode> input)
        {
            var numbers = input.ToArray();
            return Enumerable
                .Range(0, numbers.Length)
                .SelectMany(i => Enumerable
                                .Range(0, numbers.Length)
                                .Where(j => j != i)
                                .Select(j => (i, j)))
                .AsParallel()
                .Select(t => numbers[t.i].Add(numbers[t.j]).Reduce().Magnitude)
                .Max();
        }

        private static IEnumerable<TreeNode> ParseInput(string[] input)
        {
            foreach (var line in input)
            {
                yield return TreeNode.ParseNumber(line);
            }
        }

        private class TreeNode
        {
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
            public TreeNode Parent { get; set; }

            public int Value { get; set; }

            public bool IsNumber { get; set; }

            public static TreeNode FromNumber(int number) => new() { Left = null, Right = null, Value = number, IsNumber = true };

            public static TreeNode FromNumbers(int left, int right) =>
                FromNodes(FromNumber(left), FromNumber(right));

            public static TreeNode FromNumberAndNode(int left, TreeNode right) =>
                FromNodes(new() { Left = null, Right = null, Value = left, IsNumber = true }, right);

            public static TreeNode FromNumberAndNode(TreeNode left, int right) =>
                FromNodes(left, new() { Left = null, Right = null, Value = right, IsNumber = true });

            public static TreeNode FromNodes(TreeNode left, TreeNode right)
            {
                TreeNode ret = new()
                {
                    Left = left,
                    Right = right,
                    Value = -1,
                    Parent = null,
                    IsNumber = false
                };
                left.Parent = ret;
                right.Parent = ret;
                return ret;
            }
            public TreeNode Clone()
            {
                if (IsNumber)
                {
                    return FromNumber(Value);
                }

                return FromNodes(Left?.Clone(), Right?.Clone());
            }

            public TreeNode Add(TreeNode rhs) => FromNodes(Clone(), rhs.Clone());

            public TreeNode Reduce()
            {
                TreeNode nodeToExplode = null;
                do
                {
                    nodeToExplode = null;
                    Walk(this, t =>
                    {
                        if (t.curNode.IsNumber)
                        {
                            return true;
                        }
                        if (t.curNode.Left.IsNumber && t.curNode.Right.IsNumber && t.depth >= 4)
                        {
                            nodeToExplode = t.curNode;
                            return false;
                        }

                        return true;
                    });
                    if (nodeToExplode == null)
                    {
                        if (SplitFirst())
                        {
                            continue;
                        }
                        break;
                    }
                    TreeNode leftmost = null, rightmost = null;
                    var leftP = FindFirstNonAncestor(nodeToExplode, false);
                    if (leftP != null)
                    {
                        leftmost = FindFirstRegularNumber(leftP, true);
                    }
                    var rightP = FindFirstNonAncestor(nodeToExplode, true);
                    if (rightP != null)
                    {
                        rightmost = FindFirstRegularNumber(rightP, false);
                    }

                    var zero = FromNumber(0);
                    zero.Parent = nodeToExplode.Parent;

                    if (leftmost != null)
                    {
                        leftmost.Value += nodeToExplode.Left.Value;
                    }
                    if (rightmost != null)
                    {
                        rightmost.Value += nodeToExplode.Right.Value;
                    }

                    if (nodeToExplode.Parent.Right == nodeToExplode)
                    {
                        nodeToExplode.Parent.Right = zero;
                    }

                    if (nodeToExplode.Parent.Left == nodeToExplode)
                    {
                        nodeToExplode.Parent.Left = zero;
                    }
                } while (true);
                return this;
            }

            private bool SplitFirst()
            {
                TreeNode node = null;
                Walk(this, t =>
                {
                    if (t.curNode.IsNumber && t.curNode.Value >= 10)
                    {
                        node = t.curNode;
                        return false;
                    }
                    return true;
                });

                if (node == null)
                {
                    return false;
                }

                var pair = Split(node.Value);
                if (node.Parent == null)
                {
                    this.Right = pair.Right;
                    this.Left = pair.Left;
                    return false;
                }
                pair.Parent = node.Parent;
                if (node.Parent.Right == node)
                {
                    node.Parent.Right = pair;
                }
                else
                {
                    node.Parent.Left = pair;
                }

                return true;
            }

            private static TreeNode Split(int number)
            {
                if (number % 2 == 0)
                {
                    return FromNumbers(number / 2, number / 2);
                }
                return FromNumbers(number / 2, (number / 2) + 1);
            }

            public static TreeNode ParseNumber(string input)
            {
                Stack<int> openBrackets = new();
                Stack<TreeNode> nodesStack = new();
                for (var idx = 0; idx < input.Length; idx++)
                {
                    if (input[idx] == '[')
                    {
                        openBrackets.Push(idx);
                        continue;
                    }
                    if (input[idx] != ']')
                    {
                        continue;
                    }
                    var pairStartIdx = openBrackets.Pop();

                    var pairSpan = input.AsSpan(pairStartIdx + 1, idx - pairStartIdx - 1);

                    if (!pairSpan.Contains('['))
                    {
                        //simple pair
                        var t = pairSpan.IndexOf(',');
                        var pair = FromNumbers(int.Parse(pairSpan[..t]), int.Parse(pairSpan[(t + 1)..]));
                        nodesStack.Push(pair);
                        continue;
                    }
                    else
                    {
                        // contains other computed pairs
                        // at least one side is a pair computed in nodesStack, check for dangling number
                        var leftSlice = pairSpan[..pairSpan.IndexOf(',')];
                        var rightSlice = pairSpan[(pairSpan.LastIndexOf(',') + 1)..];
                        TreeNode pair;
                        if (!leftSlice.Contains('['))
                        {
                            pair = FromNumberAndNode(int.Parse(leftSlice), nodesStack.Pop());
                        }
                        else
                        {
                            if (!rightSlice.Contains(']'))
                            {
                                pair = FromNumberAndNode(nodesStack.Pop(), int.Parse(rightSlice));
                            }
                            else
                            {
                                var r = nodesStack.Pop();
                                var l = nodesStack.Pop();
                                pair = FromNodes(l, r);
                            }
                        }
                        nodesStack.Push(pair);
                    }
                }
                if (nodesStack.Count != 1)
                {
                    throw new Exception("Invalid input!");
                }
                return nodesStack.Pop();
            }

            private static bool Walk(TreeNode root, Predicate<(TreeNode curNode, int depth)> shouldContinuePredicate, int depth = 0)
            {
                if (root == null)
                {
                    return true;
                }
                if (!Walk(root.Left, shouldContinuePredicate, depth + 1))
                {
                    return false;
                }

                if (!shouldContinuePredicate((root, depth)))
                {
                    return false;
                }

                if (!Walk(root.Right, shouldContinuePredicate, depth + 1))
                {
                    return false;
                }
                return true;
            }

            private static TreeNode FindFirstNonAncestor(TreeNode node, bool pickRight = false)
            {
                if (node == null || node.Parent == null)
                {
                    return null;
                }
                if (pickRight)
                {
                    if (node.Parent.Right == node)
                    {
                        return FindFirstNonAncestor(node.Parent, true);
                    }
                    return node.Parent.Right;
                }
                else
                {
                    if (node.Parent.Left == node)
                    {
                        return FindFirstNonAncestor(node.Parent, false);
                    }
                    return node.Parent.Left;
                }
            }
            private static TreeNode FindFirstRegularNumber(TreeNode root, bool walkRight = false)
            {
                if (root == null || root.IsNumber)
                {
                    return root;
                }

                TreeNode foundNode = null;
                if (walkRight)
                {
                    foundNode = FindFirstRegularNumber(root.Right, true);
                }
                else
                {
                    foundNode = FindFirstRegularNumber(root.Left, false);
                }

                if (foundNode != null)
                {
                    return foundNode;
                }

                return FindFirstRegularNumber(root.Parent, walkRight);
            }

            public long Magnitude
            {
                get
                {
                    if (IsNumber)
                    {
                        return Value;
                    }
                    long r = 0;
                    long l = 0;
                    if (Right != null)
                    {
                        r = 2 * Right.Magnitude;
                    }
                    if (Left != null)
                    {
                        l = 3 * Left.Magnitude;
                    }
                    return l + r;
                }
            }

            public override string ToString()
            {
                if (IsNumber)
                {
                    return Value.ToString();
                }
                var l = Left == null ? "null" : Left.ToString();
                var r = Right == null ? "null" : Right.ToString();
                return $"[{l},{r}]";
            }
        }
    }
}

