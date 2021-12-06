using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace _2019
{
    public class Day17
    {
        [Fact]
        public void Test1()
        {
            string[] photo = new[] {
                "..#..........",
                "..#..........",
                "#######...###",
                "#.#...#...#.#",
                "#############",
                "..#...#...#..",
                "..#####...^.."
            };
            Assert.Equal(76, Part1(photo));
        }

        [Fact]
        public async Task Part1TestAsync()
        {
            var emulator = new IntCodeEmulator(File.ReadAllText("input/day17.txt").Split(',').Select(long.Parse).ToArray());
            Assert.Equal(4864, Part1(await GetPhotoAsync(emulator)));
        }

        [Fact]
        public void Test2()
        {
            string[] photo = new[] {
                "#######...#####",
                "#.....#...#...#",
                "#.....#...#...#",
                "......#...#...#",
                "......#...###.#",
                "......#.....#.#",
                "^########...#.#",
                "......#.#...#.#",
                "......#########",
                "........#...#..",
                "....#########..",
                "....#...#......",
                "....#...#......",
                "....#...#......",
                "....#####......"
            };
            Assert.Equal(new[] { "R", "8", "R", "8", "R", "4", "R", "4", "R", "8", "L", "6", "L", "2", "R", "4", "R", "4", "R", "8", "R", "8", "R", "8", "L", "6", "L", "2" }, GetPathCommands(photo).ToArray());
        }

        [Fact]
        public async Task Part2TestAsync()
        {
            var emulator = new IntCodeEmulator(File.ReadAllText("input/day17.txt").Split(',').Select(long.Parse).ToArray());
            var photo = await GetPhotoAsync(emulator);
            /*
            photo:
            ......#########..............................
            ......#.......#..............................
            ......#.......#.......................######^
            ......#.......#.......................#......
            ......#.......#.......................#......
            ......#.......#.......................#......
            ......#####...#...........#############......
            ..........#...#...........#..................
            ......###########.........#..................
            ......#...#...#.#.........#..................
            ......#...#...#.#.........#..................
            ......#...#...#.#.........#..................
            ......#...#...#############..................
            ......#...#.....#............................
            ......#######...#............................
            ..........#.#...#............................
            ..........#.#...#............................
            ..........#.#...#............................
            ..........#######............................
            ............#................................
            ............#................................
            ............#................................
            ............#...........#######..............
            ............#...........#.....#..............
            ........###########.....#.....#..............
            ........#...#.....#.....#.....#..............
            #############.....#.....#.....#...#..........
            #.......#.........#.....#.....#...#..........
            #.......#.....#############...#...#..........
            #.......#.....#...#.....#.#...#...#..........
            #.....#############.....#.#...#...#..........
            #.....#.#.....#.........#.#...#...#..........
            #.....#.#.....#.........###########..........
            #.....#.#.....#...........#...#..............
            #######.#######...........#...#####..........
            ..........................#.......#..........
            ..........................#.......#..........
            ..........................#.......#..........
            ..........................#.......#..........
            ..........................#.......#..........
            ..........................#########..........
            path:
            ---------------------
            L,6,L,4,R,12,L,6,R,12,R,12,L,8,L,6,L,4,R,12,L,6,L,10,L,10,L,6,L,6,R,12,R,12,L,8,L,6,L,4,R,12,L,6,L,10,L,10,L,6,L,6,R,12,R,12,L,8,L,6,L,4,R,12,L,6,L,10,L,10,L,6
            -------
        A   L,6,L,4,R,12
        B   L,6,R,12,R,12,L,8
        A   L,6,L,4,R,12
        C   L,6,L,10,L,10,L,6
        B   L,6,R,12,R,12,L,8
        A   L,6,L,4,R,12
        C   L,6,L,10,L,10,L,6
        B   L,6,R,12,R,12,L,8
        A   L,6,L,4,R,12
        C   L,6,L,10,L,10,L,6
            */

            var instructions = new[]
            {
                "A,B,A,C,B,A,C,B,A,C",
                "L,6,L,4,R,12",
                "L,6,R,12,R,12,L,8",
                "L,6,L,10,L,10,L,6"
            }.Aggregate((a, b) => a + '\n' + b) + "\nn\n";

            var emulator2 = new IntCodeEmulator(File.ReadAllText("input/day17.txt").Split(',').Select(long.Parse).ToArray());
            emulator2.WriteMemory(0, 2);

            var instructionIdx = 0;
            var dustCount = 0;

            StringBuilder sb = new StringBuilder();

            await emulator2.RunAsync(new IntCodeEmulator.SyncIO(
                () =>
                {
                    return instructions[instructionIdx++];
                },
                (value) =>
                {
                    dustCount = (int)value;
                }
            ), default);

            Assert.Equal(840248, dustCount);
        }

        private static async Task<string[]> GetPhotoAsync(IntCodeEmulator robot)
        {
            List<StringBuilder> rawOutput = new();
            rawOutput.Add(new());

            await robot.RunAsync(new IntCodeEmulator.SyncIO(
                () =>
                {
                    return 0;
                },
                (value) =>
                {
                    if (value == 10)
                    {
                        rawOutput.Add(new());
                    }
                    else
                    {
                        rawOutput[rawOutput.Count - 1].Append((char)value);
                    }
                }
            ), default);

            return rawOutput.Where(sb => sb.Length > 0).Select(sb => sb.ToString()).ToArray();
        }

        private static int Part1(string[] photo)
        {
            var width = photo[0].Length;
            var height = photo.Length;
            var sum = 0;
            for (var i = 1; i < height - 1; i++)
            {
                for (var j = 1; j < width - 1; j++)
                {
                    if (photo[i][j] == '#' &&
                        photo[i - 1][j] == '#' && photo[i + 1][j] == '#' &&
                        photo[i][j - 1] == '#' && photo[i][j + 1] == '#')
                    {
                        sum += i * j;
                    }
                }
            }
            return sum;
        }

        private static IEnumerable<string> GetPathCommands(string[] photo)
        {
            var width = photo[0].Length;
            var height = photo.Length;
            int posX = 0, posY = 0;
            int directionX = 0, directionY = -1; // up

            for (posY = 0; posY < height; posY++)
            {
                posX = photo[posY].IndexOf('^');
                if (posX != -1)
                {
                    break;
                }
            }
            var newDirection = FindRotation(photo, posX, posY, directionX, directionY);
            yield return newDirection.rotation.ToString();
            directionX = newDirection.directionX;
            directionY = newDirection.directionY;
            posX += directionX;
            posY += directionY;

            while (photo[posY][posX] != '.')
            {
                int stepCount;
                for (stepCount = 1; ; stepCount++)
                {
                    posX += directionX;
                    posY += directionY;
                    if (posX == width || posX == -1 || posY == height || posY == -1 || photo[posY][posX] != '#')
                    {
                        // end of scaffold, backtrack and break
                        posX -= directionX;
                        posY -= directionY;
                        break;
                    }
                }

                yield return stepCount.ToString();

                // rotate
                newDirection = FindRotation(photo, posX, posY, directionX, directionY);
                if (newDirection.directionX == 0 && newDirection.directionY == 0)
                {
                    break; // done
                }
                yield return newDirection.rotation.ToString();

                directionX = newDirection.directionX;
                directionY = newDirection.directionY;
                posX += directionX;
                posY += directionY;
            }
        }

        private static (char rotation, int directionX, int directionY) FindRotation(string[] photo, int posX, int posY, int curDirectionX, int curDirectionY)
        {
            var width = photo[0].Length;
            var height = photo.Length;
            if (curDirectionX != 1 && posX >= 1 && photo[posY][posX - 1] == '#')
            {
                if (curDirectionY == 1) // going south, rotate right
                {
                    return ('R', -1, 0);
                }
                else // going north, rotate left
                {
                    return ('L', -1, 0);
                }
            }
            if (curDirectionX != -1 && posX < width - 1 && photo[posY][posX + 1] == '#')
            {
                if (curDirectionY == 1) // going south, rotate left
                {
                    return ('L', 1, 0);
                }
                else // going north, rotate right
                {
                    return ('R', 1, 0);
                }
            }
            if (curDirectionY != 1 && posY >= 1 && photo[posY - 1][posX] == '#')
            {
                if (curDirectionX == 1) // going east, rotate left
                {
                    return ('L', 0, -1);
                }
                else // going west, rotate right
                {
                    return ('R', 0, -1);
                }
            }
            if (curDirectionY != -1 && posY < height - 1 && photo[posY + 1][posX] == '#')
            {
                if (curDirectionX == 1) // going east, rotate right
                {
                    return ('R', 0, 1);
                }
                else // going west, rotate left
                {
                    return ('L', 0, 1);
                }
            }
            return ('L', 0, 0); // nowhere to go
        }

    }
}
