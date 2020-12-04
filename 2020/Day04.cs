using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace _2020
{
    public class Day04
    {
        [Fact]
        public void Test1()
        {
            var input = @"
ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";
            Assert.Equal(2, Part1(input));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(254, Part1(File.ReadAllText("input/day04.txt")));
        }

        [Fact]
        public void Test3()
        {
            var input = @"
eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007

pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#88878a
hgt:164cm byr:2001 iyr:2015 cid:88
pid:045766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
            Assert.Equal(4, Part2(input));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(184, Part2(File.ReadAllText("input/day04.txt")));
        }

        private static int Part1(string input) => ParseValidPassports(input).Count();

        private static int Part2(string input)
        {
            var colorRegex = new Regex("^#[0-9a-f]{6}$");
            var pidRegex = new Regex("^[0-9]{9}$");
            return ParseValidPassports(input)
                .Where(p => IsInRange(p["byr"], 1920, 2002, 4))
                .Where(p => IsInRange(p["iyr"], 2010, 2020, 4))
                .Where(p => IsInRange(p["eyr"], 2020, 2030, 4))
                .Where(p => IsInRange(p["byr"], 1920, 2002, 4))
                .Where(p => IsLength(p["hgt"], 150, 193, 59, 76))
                .Where(p => colorRegex.IsMatch(p["hcl"]))
                .Where(p => new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(p["ecl"]))
                .Where(p => pidRegex.IsMatch(p["pid"]))
                .Count();
        }

        private static bool IsInRange(string input, int min, int max, int digitCount) =>
            input.Length == digitCount && int.Parse(input) >= min && int.Parse(input) <= max;

        private static bool IsLength(string input, int minCm, int maxCm, int minIn, int maxIn) =>
            (input.EndsWith("cm") && int.Parse(input.AsSpan(0, input.Length - 2)) >= minCm && int.Parse(input.AsSpan(0, input.Length - 2)) <= maxCm) ||
            (input.EndsWith("in") && int.Parse(input.AsSpan(0, input.Length - 2)) >= minIn && int.Parse(input.AsSpan(0, input.Length - 2)) <= maxIn);


        private static IEnumerable<Dictionary<string, string>> ParseValidPassports(string input) =>
            string.Concat(input.Where(c => c != '\r')).Split("\n\n")
                .Select(s => s.Replace('\n', ' '))
                .Select(passport => passport.Split(' ').Where(s => s.Contains(':'))
                        .Select(field => (field.Split(":")[0], field.Split(":")[1]))
                        .Append(("cid", "optional"))
                        .ToHashSet())
                .Where(set => set.Select(i => i.Item1)
                    .Intersect(new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" })
                    .Count() == 8)
                .Select(set =>
                {
                    var dict = new Dictionary<string, string>();
                    foreach (var field in set)
                    {
                        if (!dict.ContainsKey(field.Item1))
                        {
                            dict.Add(field.Item1, field.Item2);
                        }
                    }
                    return dict;
                });

    }
}
