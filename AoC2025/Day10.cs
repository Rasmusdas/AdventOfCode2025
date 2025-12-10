
using Google.OrTools.Init;
using Google.OrTools.Sat;
namespace AoC2025
{
    public class Day10 : Day
    {
        public static int[] numberToMask;
        public static Int128[] joltageToMask;

        public static Int128 overshot;

        public Day10() : base()
        {
            int max = 0;
            foreach (var line in inputLines)
            {
                var parts = line.Split(" ");
                max = Math.Max(max, parts[0].Length - 2);
            }

            numberToMask = new int[max];
            joltageToMask = new Int128[max];

            for (int i = 0; i < max; i++)
            {
                numberToMask[i] = 1 << (i);
                joltageToMask[i] = ((Int128)1) << (i * 10);
                overshot += ((Int128)1) << (i * 10) + 9;
            }            
        }

        public void SolvePart1()
        {
            int sum = 0;

            foreach (var line in inputLines)
            {
                Dictionary<int, int> stateToCount = new();
                Queue<IndicatorMachine> machines = new Queue<IndicatorMachine>();

                machines.Enqueue(new IndicatorMachine(line));

                while (machines.Count > 0)
                {
                    var nextMachine = machines.Dequeue();

                    if (nextMachine.IsCorrect) { sum += nextMachine.changes; break; }

                    for (int i = 0; i < nextMachine.stateChanges.Count; i++)
                    {
                        var newMachine = nextMachine;

                        newMachine.ChangeState(i);

                        if (stateToCount.TryGetValue(newMachine.state, out int newStateCount))
                        {
                            if (newStateCount <= newMachine.changes)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            stateToCount[newMachine.state] = newMachine.changes;
                        }

                        machines.Enqueue(newMachine);
                    }
                }
            }

            Console.WriteLine(sum);
        }

        public void SolvePart2()
        {

            int sum = 0;

            foreach(var line in inputLines)
            {
                var testMachine = new JoltageMachine(line);
                CpModel model = new CpModel();
                int n = testMachine.n;
                int m = testMachine.m;

                var M = testMachine.matrix;
                var y = testMachine.resultVector;

                IntVar[] x = new IntVar[n];
                for (int i = 0; i < n; i++)
                {
                    x[i] = model.NewIntVar(0, int.MaxValue, $"x_{i}");
                }

                for (int j = 0; j < m; j++)
                {
                    LinearExpr expr = LinearExpr.Sum(
                        System.Linq.Enumerable.Range(0, n).Select(i => x[i] * M[i, j])
                    );
                    model.Add(expr == y[j]);
                }

                model.Minimize(LinearExpr.Sum(x));

                CpSolver solver = new CpSolver();
                CpSolverStatus status = solver.Solve(model);

                if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
                {

                    for (int i = 0; i < n; i++)
                    {
                        int val = (int)solver.Value(x[i]);
                        sum += val;
                    }
                }
                else
                {
                    Console.WriteLine("No solution found.");
                }
            }

            Console.WriteLine($"Sum = {sum}");
        }

    }

    public struct IndicatorMachine
    {
        public int state;
        public int targetState;
        public int changes;
        public List<int> stateChanges;

        public IndicatorMachine(string descriptor)
        {
            stateChanges = new List<int>();

            var descriptors = descriptor.Split(" ");

            var stateDescriptor = descriptors[0].Substring(1, descriptors[0].Length - 2);

            int num = 0;

            for (int i = 0; i < stateDescriptor.Length; i++)
            {
                if (stateDescriptor[i] == '#')
                {
                    num |= 1 << (i);
                }
            }

            targetState = num;

            for (int i = 1; i < descriptors.Length - 1; i++)
            {
                int buttonChange = 0;
                var buttonDescriptors = descriptors[i].Substring(1, descriptors[i].Length - 2).Split(",");

                foreach (var buttonNumber in buttonDescriptors)
                {
                    buttonChange |= Day10.numberToMask[int.Parse(buttonNumber)];
                }

                stateChanges.Add(buttonChange);
            }

        }

        public void ChangeState(int stateIndex)
        {
            state ^= stateChanges[stateIndex];
            changes++;
        }

        public bool IsCorrect => state == targetState;
    }


    // Graph + Memoization = Never gonna finish :sadge:
    public struct JoltageMachineGraph
    {
        public Int128 targetState;
        public int changes;
        public List<Int128> stateChanges;
        public Int128 state;
        public int maxStates;

        public JoltageMachineGraph(string descriptor)
        {
            stateChanges = new List<Int128>();

            var descriptors = descriptor.Split(" ");

            var stateDescriptor = descriptors[^1].Substring(1, descriptors[^1].Length - 2).Split(",");

            Int128 num = 0;

            for (int i = 0; i < stateDescriptor.Length; i++)
            {
                num += Day10.joltageToMask[i] * int.Parse(stateDescriptor[i]);
                maxStates += int.Parse(stateDescriptor[i]);
            }

             targetState = num;

            stateChanges = new();
            for (int i = 1; i < descriptors.Length - 1; i++)
            {
                var buttonDescriptors = descriptors[i].Substring(1, descriptors[i].Length - 2).Split(",");

                Int128 stateChange = 0;
                int buttons = buttonDescriptors.Length;

                foreach (var buttonNumber in buttonDescriptors)
                {
                    stateChange += Day10.joltageToMask[int.Parse(buttonNumber)];

                }
                stateChanges.Add(stateChange);

            }

        }

        public void ChangeState(int stateIndex)
        {
            state += stateChanges[stateIndex];
            changes++;
        }

        public bool IsCorrect => state == targetState;

        public bool HasOvershot()
        {
            return ((targetState - state) & Day10.overshot) != 0;
        }
        public Int128 GetTargetDifference()
        {
            Int128 res = 0;
            
            for (int i = 0; i < 10; i++)
            {
                
                res += (targetState - state) >> (i * 10) & 1023;
            }

            return res;
        }

        public void PrintRemainders()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine((state) >> (i * 10) & 1023);
            }
        }
    }

    public struct JoltageMachine
    {
        public int[,] matrix;
        public int[] resultVector;

        public int n;
        public int m;
        public JoltageMachine(string descriptor)
        {
            var descriptors = descriptor.Split(" ");

            var stateDescriptor = descriptors[^1].Substring(1, descriptors[^1].Length - 2).Split(",");

            matrix = new int[descriptors.Length-2, stateDescriptor.Length];

            n = descriptors.Length - 2;
            m = stateDescriptor.Length;

            for (int i = 1; i < descriptors.Length-1; i++)
            {
                var numbers = descriptors[i].Substring(1, descriptors[i].Length - 2).Split(",");

                foreach(var number in numbers)
                {
                    matrix[i-1, int.Parse(number)] = 1;
                }
            }

            resultVector = new int[stateDescriptor.Length];

            for (int i = 0; i < stateDescriptor.Length; i++)
            {
                resultVector[i] = int.Parse(stateDescriptor[i]);
            }

            //for (int i = 0; i < matrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < matrix.GetLength(1); j++)
            //    {
            //        Console.Write(matrix[i, j]);
            //        Console.Write(" ");
            //    }
            //    Console.WriteLine();
            //}
            //for (int i = 0; i < resultVector.Length; i++)
            //{
            //    Console.WriteLine(resultVector[i]);
            //}
        }



    }
}

