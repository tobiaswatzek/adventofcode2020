using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adventofcode2020.Days
{
    public class Day13 : IDay
    {
        public int Number { get; } = 13;
        public async Task<(string firstSolution, string secondSolution)> Solve()
        {
            var (timestamp, ids) = await ParseInputForFirst();
            var firstSolution = SolveFirst(timestamp, ids);

            return (firstSolution, "");
        }

        private static string SolveFirst(int timestamp, IEnumerable<int> ids)
        {
            var (id, nextDeparture) = ids.Select(id => (id, nextDeparture: (int)(Math.Ceiling(timestamp / (double)id) * id)))
                .OrderBy(t => t.nextDeparture)
                .First();

            var minutesToWait = nextDeparture - timestamp;

            return (id * minutesToWait).ToString();
        }

        private static async Task<(int timestamp, int[] ids)> ParseInputForFirst()
        {
            var lines = await File.ReadAllLinesAsync("./input/day13.txt");
            var timestamp = int.Parse(lines[0]);
            var ids = lines[1]
                .Replace("x", "")
                .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            return (timestamp, ids);
        }
    }
}
