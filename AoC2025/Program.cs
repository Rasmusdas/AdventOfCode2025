using AoC2025;
using BenchmarkDotNet.Running;

#if DEBUG
new Day1().SolvePart1();
new Day1().SolvePart2();

new Day2().SolvePart1();
new Day2().SolvePart2();

new Day3().SolvePart1();
new Day3().SolvePart2();

new Day4().SolvePart1();
new Day4().SolvePart2();

new Day5().SolvePart1();
new Day5().SolvePart2();

#else
// BenchmarkRunner.Run<Day1>();
BenchmarkRunner.Run<Day2>();
BenchmarkRunner.Run<Day3>();
BenchmarkRunner.Run<Day4>();
// BenchmarkRunner.Run<Day5>();
#endif