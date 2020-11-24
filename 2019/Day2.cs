using System;
using System.IO;
using System.Linq;
using Xunit;

namespace _2019
{
    public class Day2
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, SolutionBeforePatch(new int[] { 1, 0, 0, 0, 99 }));
            Assert.Equal(2, SolutionBeforePatch(new int[] { 2, 3, 0, 3, 99 }));
            Assert.Equal(3500, SolutionBeforePatch(new int[] { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }));
            Assert.Equal(30, SolutionBeforePatch(new int[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }));
        }

        [Fact]
        public void Test2()
        {
            var input = File.ReadAllText("input/day2.txt").Split(',').Select(int.Parse).ToArray();
            input[1] = 12;
            input[2] = 2;
            Assert.Equal(3790689, SolutionBeforePatch(input));
        }

        [Fact]
        public void Test3()
        {
            var input = File.ReadAllText("input/day2.txt").Split(',').Select(int.Parse).ToArray();

            Assert.Equal(6533, Solution2(input));
        }

        private int SolutionBeforePatch(int[] input)
        {
            for (var i = 0; i < input.Length; i += 4)
            {
                var opCode = input[i];
                var operand1 = (i + 1 < input.Length) ? input[i + 1] : 0;
                var operand2 = (i + 2 < input.Length) ? input[i + 2] : 0;
                var destination = (i + 3 < input.Length) ? input[i + 3] : 0;
                switch (opCode)
                {
                    case 1:
                        input[destination] = input[operand1] + input[operand2];
                        break;
                    case 2:
                        input[destination] = input[operand1] * input[operand2];
                        break;
                    case 99:
                        return input[0];
                    default:
                        throw new Exception("Invalid op code!");
                }
            }
            throw new Exception("The program didn't halt!");
        }

        private int Solution2(int[] input)
        {
            int[] bkp = new int[input.Length];
            input.CopyTo(bkp, 0);

            for (var i = 0; i <= 99; i++)
            {
                for(var j = 0; j <= 99; j++)
                {
                    input[1] = i;
                    input[2] = j;
                    if (19690720 == SolutionBeforePatch(input))
                    {
                        return 100 * i + j;
                    }
                    bkp.CopyTo(input, 0);
                }
            }
            return 0;
        }
    }
}
