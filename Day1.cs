using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adventofcode2020
{
    public class Day1
    {
        public async Task Solve()
        {
            var lines = await File.ReadAllLinesAsync("./input/day1.txt");

            var numbers = lines.Select(l => int.Parse(l)).ToList();

            var first = SolveFirst(numbers);
            Console.WriteLine($"Day 1 (1): {first}");

            var second = SolveSecond(numbers);
            Console.WriteLine($"Day 1 (2): {second}");
        }

        private string SolveFirst(List<int> numbers)
        {
            var tuple = numbers.SelectMany(x => numbers.Select(y => (x: x, y: y)).Where(t => (t.x + t.y) == 2020))
                .FirstOrDefault();
            var result = tuple.x * tuple.y;

            return result.ToString();
        }

        private string SolveSecond(List<int> numbers)
        {
            var tuple = numbers.SelectMany(x => numbers
                    .SelectMany(y => numbers.Select(z => (x: x, y: y, z: z)))
                    .Where(t => (t.x + t.y + t.z) == 2020))
                .FirstOrDefault();
            var result = tuple.x * tuple.y * tuple.z;

            return result.ToString();
        }
    }
}
