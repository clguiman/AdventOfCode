using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _2018
{
    public class Day04
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(240, Part1(ParseInput(new[] {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up"
            })));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(4716, Part1(ParseInput(File.ReadAllLines("input/day04.txt"))));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(4455, Part2(ParseInput(new[] {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up"
            })));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(117061, Part2(ParseInput(File.ReadAllLines("input/day04.txt"))));
        }
        private static long Part1(List<GuardActions> input)
        {
            var mostAsleep = ComputeSleepSchedule(input).Values.OrderByDescending(t => t.totalSleepTime).First();

            return mostAsleep.id * mostAsleep.schedule.Select((count, idx) => (idx, count)).OrderByDescending(t => t.count).First().idx;
        }

        private static long Part2(List<GuardActions> input)
        {
            var mostAsleep = ComputeSleepSchedule(input).Values
                                    .Select(v => v.schedule
                                            .Select((count, timeIdx) => (v.id, timeIdx, count))
                                            .OrderByDescending(t => t.count).First())
                                    .OrderByDescending(t => t.count).First();

            return mostAsleep.id * mostAsleep.timeIdx;
        }

        private static Dictionary<int, (int id, int[] schedule, int totalSleepTime)> ComputeSleepSchedule(List<GuardActions> input)
        {
            var sleepTime = input.Select(g => g.Id).Distinct().ToDictionary(id => id, id => (id, schedule: new int[60], totalSleepTime: 0));
            for (var idx = 0; idx < input.Count - 1; idx++)
            {
                if (input[idx].Type != GuardActions.ActionType.FallAsleep)
                {
                    continue;
                }
                if (input[idx + 1].Type != GuardActions.ActionType.WakeUp)
                {
                    throw new Exception("Expecting the next action to be 'wake up'!");
                }

                var startMinute = input[idx].Timestamp.Minute;
                idx++;
                var endMinute = input[idx].Timestamp.Minute;
                var t = sleepTime[input[idx].Id];
                for (var i = startMinute; i < endMinute; i++)
                {
                    t.schedule[i]++;
                }
                t.totalSleepTime += endMinute - startMinute;
                sleepTime[input[idx].Id] = t;
            }
            return sleepTime;
        }

        private static List<GuardActions> ParseInput(IEnumerable<string> input)
        {
            var sortedInput = input.Select(line =>
            {
                var s = line.Split(']');
                var time = DateTime.Parse(s[0][1..]);
                return (time, s[1].Trim());
            }).OrderBy(t => t.time);

            var lastGuardId = -1;
            List<GuardActions> guardActions = new();
            foreach ((var time, var line) in sortedInput)
            {
                var actionStr = line;
                if (line.Contains('#'))
                {
                    var s2 = line[(line.IndexOf('#') + 1)..].Split(' ');
                    lastGuardId = int.Parse(s2[0]);
                    actionStr = line[(line.IndexOf('#') + 1)..][s2[0].Length..].Trim();
                }
                var action = GuardActions.ActionType.Invalid;
                action = actionStr switch
                {
                    "wakes up" => GuardActions.ActionType.WakeUp,
                    "begins shift" => GuardActions.ActionType.BeginsShift,
                    "falls asleep" => GuardActions.ActionType.FallAsleep,
                    _ => throw new ArgumentException($"Invalid action: {actionStr}"),
                };
                guardActions.Add(new(lastGuardId, time, action));
            }
            return guardActions.OrderBy(g => g.Timestamp).ToList();
        }

        private class GuardActions
        {
            public enum ActionType
            {
                Invalid,
                BeginsShift,
                FallAsleep,
                WakeUp
            }

            public GuardActions(int id, DateTime timestamp, ActionType type)
            {
                Timestamp = timestamp;
                Id = id;
                Type = type;
            }

            public DateTime Timestamp { get; }
            public int Id { get; }
            public ActionType Type { get; }

            public override string ToString() => $"{Timestamp} {Id} {Type}";
        }
    }
}
