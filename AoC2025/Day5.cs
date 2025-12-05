using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day5 : Day
    {

        public void SolvePart1()
        {
            List<Interval> intervals = new List<Interval>();

            int validIds = 0;

            var intervalSplit = inputLines.Index().First((x) => x.Item.Length == 0).Index;

            for (int i = 0; i < intervalSplit; i++)
            {
                var split = inputLines[i].Split('-');
                    
                long start = long.Parse(split[0]);
                long end = long.Parse(split[1]);

                intervals.Add(new Interval(start, end));
            }

            intervals.Sort((x, y) => x.Start.CompareTo(y.Start));

            int intervalCount = 0;
            
            while (intervalCount < intervals.Count-1)
            {
                
                
                if (intervals[intervalCount].End > intervals[intervalCount + 1].Start)
                {
                    intervals[intervalCount].End = Math.Max(intervals[intervalCount].End, intervals[intervalCount + 1].End);
                    intervals.RemoveAt(intervalCount+1);
                }
                else
                {
                    intervalCount++;
                }
            }

            for (int i = intervalSplit+1; i < inputLines.Length; i++)
            {
                var id = long.Parse(inputLines[i]);
                foreach (var interval in intervals)
                {
                    if (id >= interval.Start &&  id <= interval.End)
                    {
                        validIds++;
                    }
                }
            }
            
            Console.WriteLine(validIds);
        }

        public void SolvePart2()
        {
            List<Interval> intervals = new List<Interval>();

            double validIds = 0;

            var intervalSplit = inputLines.Index().First((x) => x.Item.Length == 0).Index;
            
            
            Console.WriteLine(inputLines[intervalSplit]);

            for (int i = 0; i < intervalSplit; i++)
            {
                var split = inputLines[i].Split('-');
                    
                double start = double.Parse(split[0]);
                double end = double.Parse(split[1]);

                intervals.Add(new Interval(start, end));
            }

            intervals.Sort((x, y) => x.Start.CompareTo(y.Start));

            int intervalCount = 0;
            Console.WriteLine(intervals.Count);
            while (intervalCount < intervals.Count-1)
            {
                
                
                if (intervals[intervalCount].End > intervals[intervalCount + 1].Start)
                {
                    intervals[intervalCount].End = Math.Max(intervals[intervalCount].End, intervals[intervalCount + 1].End);
                    intervals.RemoveAt(intervalCount+1);
                }
                else
                {
                    intervalCount++;
                }
            }
            
            Console.WriteLine(intervals.Count);
            
            foreach (var interval in intervals)
            {
                validIds += interval.End-interval.Start+1;
            }

            Console.WriteLine(validIds);
        }
    }

    public class Interval(double start, double end)
    {
        public double Start = start;
        public double End = end;

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
        }
    }
}
