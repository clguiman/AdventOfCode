using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace _2019
{
    public class Day15
    {
        [Fact]
        public async Task Part1TestAsync()
        {
            var emulator = new IntCodeEmulator(File.ReadAllText("input/day15.txt").Split(',').Select(long.Parse).ToArray());
            var map = await DiscoverAsync(emulator);
            Assert.Equal(282, (int)map.Enumerate().Where(v => v.Item1.Type == VisitedLocation.LocationType.Oxygen).Select(v => v.Item1.MinStepsFromOrigin).FirstOrDefault());
        }

        [Fact]
        public async Task Part2TestAsync()
        {
            var emulator = new IntCodeEmulator(File.ReadAllText("input/day15.txt").Split(',').Select(long.Parse).ToArray());
            var map = await DiscoverAsync(emulator);

            map.RemoveIf(v => v.Type == VisitedLocation.LocationType.Wall);
            var oxygenPos = map.Enumerate().Where(v => v.Item1.Type == VisitedLocation.LocationType.Oxygen).Select(v => (v.x, v.y)).FirstOrDefault();
            map.Remove(oxygenPos.x, oxygenPos.y);

            List<(int x, int y)> nextPositions = _neighborDelta
                .Where(d =>map.ContainsPosition(oxygenPos.x + d.deltaX, oxygenPos.y + d.deltaY))
                .Select(d => (oxygenPos.x + d.deltaX, oxygenPos.y + d.deltaY))
                .ToList();

            int time = 0;
            while (!map.IsEmpty)
            {
                foreach (var (x, y) in nextPositions)
                {
                    map.Remove(x, y);
                }
                nextPositions = nextPositions
                    .SelectMany(pos => _neighborDelta.Select(d => (pos.x + d.deltaX, pos.y + d.deltaY)))
                    .Where(pos => map.ContainsPosition(pos.Item1, pos.Item2))
                    .ToList();
                time++;
            }

            Assert.Equal(286, time);
        }

        private static async Task<InfiniteMap<VisitedLocation>> DiscoverAsync(IntCodeEmulator robot)
        {
            Stack<MovementCommand> commands = new();
            commands.Push(MovementCommand.West);
            commands.Push(MovementCommand.East);
            commands.Push(MovementCommand.South);
            commands.Push(MovementCommand.North);

            InfiniteMap<VisitedLocation> map = new();
            map.SetValue(0, 0, new(VisitedLocation.LocationType.Robot, 0));

            CancellationTokenSource cts = new();
            int posX = 0, posY = 0, prevPosX = 0, prevPosY = 0;
            MovementCommand curCommand = MovementCommand.North;

            await robot.RunAsync(new IntCodeEmulator.SyncIO(
                () =>
                {
                    if (commands.Count == 0)
                    {
                        cts.Cancel();
                        return (long)MovementCommand.North;
                    }
                    var command = commands.Pop();
                    prevPosX = posX; prevPosY = posY;
                    switch (command)
                    {
                        case MovementCommand.North: { posX--; break; }
                        case MovementCommand.South: { posX++; break; }
                        case MovementCommand.East: { posY++; break; }
                        case MovementCommand.West: { posY--; break; }
                        default: throw new ArgumentException(nameof(command));
                    }
                    curCommand = command;
                    return (long)command;
                },
                (value) =>
                {
                    var status = (StatusCode)value;
                    switch (status)
                    {
                        case StatusCode.HitWall:
                            {
                                map.SetValue(posX, posY, new(VisitedLocation.LocationType.Wall, uint.MaxValue));

                                if (map.TryGetValue(prevPosX, prevPosY, out var curLocation))
                                {
                                    if (curLocation.MinStepsFromOrigin != 0)
                                    {
                                        BackTrack(map, prevPosX, prevPosY, commands);
                                    }
                                }
                                posX = prevPosX;
                                posY = prevPosY;
                                break;
                            }
                        case StatusCode.MovedOne:
                            {
                                if (!map.TryGetValue(prevPosX, prevPosY, out var prevLocation))
                                {
                                    throw new Exception("previous location should be available!");
                                }

                                if (map.TryGetValue(posX, posY, out var curLocation))
                                {
                                    // we were already here, back track
                                    if (curLocation.MinStepsFromOrigin != 0)
                                    {
                                        BackTrack(map, posX, posY, commands);
                                    }
                                }
                                else
                                {
                                    map.SetValue(posX, posY, new(VisitedLocation.LocationType.Empty, prevLocation.MinStepsFromOrigin + 1));
                                    if (curCommand != MovementCommand.South)
                                    {
                                        commands.Push(MovementCommand.North);
                                    }
                                    if (curCommand != MovementCommand.North)
                                    {
                                        commands.Push(MovementCommand.South);
                                    }
                                    if (curCommand != MovementCommand.West)
                                    {
                                        commands.Push(MovementCommand.East);
                                    }
                                    if (curCommand != MovementCommand.East)
                                    {
                                        commands.Push(MovementCommand.West);
                                    }

                                    commands.Push(curCommand);
                                }
                                break;
                            }
                        case StatusCode.MovedOnOxygenSystem:
                            {
                                if (!map.TryGetValue(prevPosX, prevPosY, out var prevLocation))
                                {
                                    throw new Exception("previous location should be available!");
                                }

                                map.SetValue(posX, posY, new(VisitedLocation.LocationType.Oxygen, prevLocation.MinStepsFromOrigin + 1));
                                break;
                            }
                        default: throw new ArgumentException(nameof(status));
                    }

                }
            ), cts.Token);

            return map;
        }

        private static void BackTrack(InfiniteMap<VisitedLocation> map, int x, int y, Stack<MovementCommand> commands)
        {
            if (!map.TryGetValue(x, y, out var curLocation))
            {
                throw new ArgumentException("We should always backtrack from a visited location!");
            }

            // first push the lowest location
            var lowestStepLocation = _neighborDelta.Select(d =>
            {
                if (!map.TryGetValue(x + d.deltaX, y + d.deltaY, out var loc))
                {
                    return (d.direction, uint.MaxValue);
                }
                return (d.direction, loc.MinStepsFromOrigin);
            })
            .Where(d => d.Item2 != uint.MaxValue)
            .OrderBy(d => d.Item2)
            .FirstOrDefault(); // we should always have a valid location 
            commands.Push(lowestStepLocation.direction);

            // push all directions to not-visited locations
            foreach (var d in _neighborDelta.Where(d => !map.TryGetValue(x + d.deltaX, y + d.deltaY, out var _)))
            {
                commands.Push(d.direction);
            }
        }

        private static readonly (int deltaX, int deltaY, MovementCommand direction)[] _neighborDelta = Enumerable.Range(-1, 3)
                .SelectMany(deltaX => Enumerable.Range(-1, 3).Select(deltaY => (deltaX, deltaY)))
                .Where(n => Math.Abs(n.deltaX) != Math.Abs(n.deltaY))
                .Select(n =>
                {
                    MovementCommand direction = MovementCommand.North;
                    if (n.deltaX == -1)
                        direction = MovementCommand.North;
                    else if (n.deltaX == 1)
                        direction = MovementCommand.South;
                    else if (n.deltaY == -1)
                        direction = MovementCommand.West;
                    else if (n.deltaY == 1)
                        direction = MovementCommand.East;
                    return (n.deltaX, n.deltaY, direction);
                })
                .ToArray();

        private class InfiniteMap<T>
        {
            public void SetValue(int x, int y, T value)
            {
                if (!_map.TryGetValue(x, out var row))
                {
                    row = new Dictionary<int, T>();
                    _map.Add(x, row);
                }

                if (row.ContainsKey(y))
                {
                    row[y] = value;
                }
                else
                {
                    row.Add(y, value);
                }
            }

            public bool ContainsPosition(int x, int y)
            {
                if (!_map.TryGetValue(x, out var row))
                {
                    return false;
                }

                return row.ContainsKey(y);
            }

            public bool TryGetValue(int x, int y, out T value)
            {
                if (!_map.TryGetValue(x, out var row))
                {
                    value = default;
                    return false;
                }

                return row.TryGetValue(y, out value);
            }

            public void Remove(int x, int y)
            {
                if (!_map.TryGetValue(x, out var row))
                {
                    return;
                }

                if (row.ContainsKey(y))
                {
                    row.Remove(y);
                }

                if (row.Count == 0)
                {
                    _map.Remove(x);
                }
            }

            public bool IsEmpty => _map.Count == 0;

            public (int minRow, int minColumn) GetMinRowAndColumnIdx() => (_map.Keys.Min(), _map.Values.Where(row => row.Count > 0).Select(row => row.Keys.Min()).Min());
            public (int maxRow, int maxColumn) GetMaxRowAndColumnIdx() => (_map.Keys.Max(), _map.Values.Where(row => row.Count > 0).Select(row => row.Keys.Max()).Max());

            public IEnumerable<(T, int x, int y)> Enumerate()
            {
                var upperLeft = GetMinRowAndColumnIdx();
                var lowerRight = GetMaxRowAndColumnIdx();
                for (var i = upperLeft.minRow; i <= lowerRight.maxRow; i++)
                {
                    for (var j = upperLeft.minColumn; j <= lowerRight.maxColumn; j++)
                    {
                        if (TryGetValue(i, j, out var val))
                        {
                            yield return (val, i, j);
                        }
                    }
                }
            }

            public void RemoveIf(Predicate<T> pred)
            {
                var upperLeft = GetMinRowAndColumnIdx();
                var lowerRight = GetMaxRowAndColumnIdx();
                for (var i = upperLeft.minRow; i <= lowerRight.maxRow; i++)
                {
                    for (var j = upperLeft.minColumn; j <= lowerRight.maxColumn; j++)
                    {
                        if (TryGetValue(i, j, out var val))
                        {
                            if (pred(val))
                            {
                                Remove(i, j);
                            }
                        }
                    }
                }
            }

            public override string ToString()
            {
                var upperLeft = GetMinRowAndColumnIdx();
                var lowerRight = GetMaxRowAndColumnIdx();
                StringBuilder sb = new();
                for (var i = upperLeft.minRow; i <= lowerRight.maxRow; i++)
                {
                    for (var j = upperLeft.minColumn; j <= lowerRight.maxColumn; j++)
                    {
                        if (!TryGetValue(i, j, out var val))
                        {
                            sb.Append(' ');
                            continue;
                        }
                        sb.Append(val.ToString());
                    }
                    sb.AppendLine();
                }

                return sb.ToString();
            }

            private Dictionary<int, Dictionary<int, T>> _map = new();
        }

        private enum StatusCode : long
        {
            HitWall = 0,
            MovedOne = 1,
            MovedOnOxygenSystem = 2
        }

        private enum MovementCommand : long
        {
            North = 1,
            South = 2,
            East = 3,
            West = 4
        }

        private struct VisitedLocation
        {
            public enum LocationType
            {
                Unknown,
                Empty,
                Wall,
                Oxygen,
                Robot
            }

            public VisitedLocation()
            {
                MinStepsFromOrigin = uint.MaxValue;
                Type = LocationType.Unknown;
            }

            public VisitedLocation(LocationType type, uint minSteps)
            {
                MinStepsFromOrigin = minSteps;
                Type = type;
            }
            public static void GetDefaultValue(StringBuilder sb)
            {
                sb.Append(' ');
            }

            public override string ToString() => Type switch
            {
                LocationType.Robot => "D",
                LocationType.Wall => "#",
                LocationType.Empty => ".",
                LocationType.Oxygen => "O",
                _ => " ",
            };

            public uint MinStepsFromOrigin;
            public LocationType Type;
        }
    }
}
