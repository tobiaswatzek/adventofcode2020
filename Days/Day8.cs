using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adventofcode2020.Days
{
    public class Day8 : IDay
    {
        public int Number { get; } = 8;

        public async Task<(string firstSolution, string secondSolution)> Solve()
        {
            var instructions = (await ParseInstructions()).ToList();
            var firstSolution = SolveFirst(instructions);

            return (firstSolution, "");
        }

        private static string SolveFirst(IReadOnlyList<Instruction> instructions)
        {
            var environment = new Environment(0);
            var instruction = instructions[0];
            while (instruction.ExecutionCount == 0)
            {
                instruction = instruction.Execute(environment, instructions);
            }

            return environment.Accumulator.ToString();
        }

        private static async Task<IEnumerable<Instruction>> ParseInstructions()
        {
            var lines = await File.ReadAllLinesAsync("./input/day8.txt");
            return lines.Select((line, address) =>
            {
                var parts = line.Split(" ", StringSplitOptions.TrimEntries);
                var instructionName = parts[0];
                var instructionValue = int.Parse(parts[1], NumberStyles.Integer);

                return instructionName switch
                {
                    "jmp" => (Instruction) new Jump(address, instructionValue),
                    "acc" => new Accumulation(address, instructionValue),
                    "nop" => new NoOperation(address, instructionValue),
                    _ => throw new InvalidOperationException($"Instruction name '{instructionName}' not found.")
                };
            });
        }
    }


    public class Environment
    {
        public long Accumulator { get; private set; }

        public Environment(long initialAccumulatorValue)
        {
            Accumulator = initialAccumulatorValue;
        }

        public void AddToAccumulator(int n)
        {
            Accumulator += n;
        }
    }

    public abstract class Instruction
    {
        public int ExecutionCount { get; set; }

        public int Address { get; }
        public int Value { get; }

        public string Name { get; }

        public Instruction(int address, string name, int value)
        {
            Name = name;
            Value = value;
            Address = address;
            ExecutionCount = 0;
        }

        public Instruction Execute(Environment environment, IReadOnlyList<Instruction> allInstructions)
        {
            ExecutionCount++;
            return ExecuteInternal(environment, allInstructions);
        }

        protected abstract Instruction ExecuteInternal(Environment environment,
            IReadOnlyList<Instruction> allInstructions);
    }

    public class Accumulation : Instruction
    {
        public Accumulation(int address, int value) : base(address, "acc", value)
        {
        }

        protected override Instruction ExecuteInternal(Environment environment,
            IReadOnlyList<Instruction> allInstructions)
        {
            environment.AddToAccumulator(Value);
            return allInstructions[Address + 1];
        }
    }

    public class Jump : Instruction
    {
        public Jump(int address, int value) : base(address, "jmp", value)
        {
        }

        protected override Instruction ExecuteInternal(Environment environment,
            IReadOnlyList<Instruction> allInstructions)
        {
            return allInstructions[Address + Value];
        }
    }


    public class NoOperation : Instruction
    {
        public NoOperation(int address, int value) : base(address, "nop", value)
        {
        }

        protected override Instruction ExecuteInternal(Environment environment,
            IReadOnlyList<Instruction> allInstructions)
        {
            return allInstructions[Address + 1];
        }
    }
}
