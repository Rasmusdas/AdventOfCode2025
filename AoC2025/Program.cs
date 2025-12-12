using AoC2025;
using BenchmarkDotNet.Running;

#if DEBUG
Console.WriteLine("Day 1");
new Day1().SolvePart1();
new Day1().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 2");
new Day2().SolvePart1();
new Day2().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 3");
new Day3().SolvePart1();
new Day3().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 4");
new Day4().SolvePart1();
new Day4().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 5");
new Day5().SolvePart1();
new Day5().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 6");
new Day6().SolvePart1();
new Day6().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 7");
new Day7().SolvePart1();
new Day7().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 8");
new Day8().SolvePart1();
new Day8().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 9");
new Day9().SolvePart1();
new Day9().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 10");
new Day10().SolvePart1();
new Day10().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 11");
new Day11().SolvePart1();
new Day11().SolvePart2();
Console.WriteLine();

Console.WriteLine("Day 12");
new Day12().SolvePart1();

#else
// BenchmarkRunner.Run<Day1>();
// BenchmarkRunner.Run<Day2>();
// BenchmarkRunner.Run<Day3>();
// BenchmarkRunner.Run<Day4>();
// BenchmarkRunner.Run<Day5>();
// BenchmarkRunner.Run<Day6>();
// BenchmarkRunner.Run<Day7>();
// BenchmarkRunner.Run<Day8>();
BenchmarkRunner.Run<Day9>();

#endif