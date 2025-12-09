using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AoC2025
{
    public class Day9 : Day
    {

        [Benchmark]
        public void SolvePart1()
        {

            List<Point> points = new List<Point>();
            foreach(var line in inputLines)
            {
                var numbers = line.Split(",");

                var point = new Point(int.Parse(numbers[0]), int.Parse(numbers[1]));

                points.Add(point);
            }

            double bestSize = 0;
            Point p1;
            Point p2;

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i+1; j < points.Count; j++)
                {
                    double size = (points[i] - points[j]).GetSize();
                    if (size > bestSize)
                    {
                        p1 = points[i];
                        p2 = points[j];
                        bestSize = size;
                    }
                }
            }
#if DEBUG
            Console.WriteLine(bestSize);
#endif
        }

        [Benchmark]
        public void SolvePart2()
        {

            Dictionary<double, bool> uniqueHorizontal = new();
            List<Point> points = new List<Point>();
            foreach (var line in inputLines)
            {
                var numbers = line.Split(",");

                var point = new Point(int.Parse(numbers[0]), int.Parse(numbers[1]));

                points.Add(point);
            }

            List<Line> horizontalLines = new();
            List<Line> verticalLines = new();

            for (int i = 0; i < points.Count; i++)
            {
                var line = new Line(points[i], points[(i + 1) % points.Count]);

                if (line.horizontal && !uniqueHorizontal.TryGetValue(line.P1.Y, out _))
                {
                    horizontalLines.Add(line);
                    uniqueHorizontal[line.P1.Y] = true;
                }
                else
                {
                    verticalLines.Add(line);
                }
            }

            horizontalLines.Sort((l1, l2) => l1.P1.Y.CompareTo(l2.P2.Y));
            verticalLines.Sort((l1, l2) => l1.P1.X.CompareTo(l2.P2.X));

            points.Sort((x, y) => x.X.CompareTo(y.X));

            double bestSize = -1;
            Box bestBox = null;

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i+1; j < points.Count; j++)
                {
                    Box box = new Box(points[i], points[j]);

                    if (box.GetSize() < bestSize) continue;

                    box.P1.X += 0.01;
                    box.P2.X -= 0.01;
                    box.P1.Y += 0.01;
                    box.P2.Y -= 0.01;


                    var p1 = box.P1;
                    var p2 = new Point(box.P2.X, box.P1.Y);
                    var p3 = new Point(box.P1.X, box.P2.Y);
                    var p4 = box.P2;

                    var h1 = new Line(p1, p2);
                    var h2 = new Line(p3, p4);

                    var v1 = new Line(p1, p3);
                    var v2 = new Line(p2, p4);


                    int h1Count = 0;
                    int h2Count = 0;
                    int v1Count = 0;
                    int v2Count = 0;

                    

                    foreach (var vTest in verticalLines)
                    {
                        if (vTest.Intersects(h1)) { h1Count++; break; }
                        if (vTest.Intersects(h2)) { h2Count++; break; }
                    }

                    if (h1Count != 0 || h2Count != 0) continue;

                    foreach (var hTest in horizontalLines)
                    {
                        if (hTest.Intersects(v1)) { v1Count++; break; }
                        if (hTest.Intersects(v2)) { v2Count++; break; }
                    }

                    if (v1Count != 0 || v2Count != 0) continue;


                    box = new Box(points[i], points[j]);

                    if (box.GetSize() > bestSize)
                    {
                        bestSize = box.GetSize();
                        bestBox = box;
                    }


                }
            }


#if DEBUG
            Console.WriteLine(bestSize);
#endif


        }
    }



    public struct Point(double x, double y)
    {
        public double X = x;
        public double Y = y;


        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public double GetSize()
        {
            return Math.Abs((X + 1) * (Y + 1));
        }

        public override string ToString()
        {
            return $"(x: {X}, y: {Y})";
        }
    }

    public class Line
    {
        public Point P1;
        public Point P2;
        public Point diff;

        public bool horizontal;
        public Line(Point p1, Point p2)
        {
            P1 = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            P2 = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));

            horizontal = (P2 - P1).X != 0;
        }

        public bool Intersects(Line other)
        {
            if (horizontal == other.horizontal) return false;

            return horizontal ? Math.Min(other.P1.Y, other.P2.Y) <= P1.Y && Math.Max(other.P1.Y, other.P2.Y) >= P1.Y && P1.X <= other.P1.X && P2.X >= other.P1.X : other.Intersects(this);

        }

        public override string ToString()
        {
            return $"(P1: {P1}, P2: {P2})";
        }
    }


    public class Box
    {
        public Point P1;
        public Point P2;

        public Box(Point p1, Point p2)
        {
            P1 = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            P2 = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));
        }

        public double GetSize()
        {

            if (P1.X < 0 || P2.X < 0 || P1.Y < 0 || P2.Y < 0) return 0;
            
            var diff = P2 - P1;
            diff.X++;
            diff.Y++;
            return diff.X * diff.Y;
        }


        public double GetOverlapSize(Box other)
        {
            return GetOverlap(other).GetSize();
        }

        public Box GetOverlap(Box other)
        {

            if(P2.X < other.P1.X || P2.Y < other.P1.Y)
            {
                return new Box(new Point(-1, -1), new Point(-1, -1));
            }

            Point min = new Point(Math.Max(P1.X, other.P1.X), Math.Max(P1.Y, other.P1.Y));
            Point max = new Point(Math.Min(P2.X, other.P2.X), Math.Min(P2.Y, other.P2.Y));

            if(min.X > max.X || min.Y > max.Y)
            {
                return new Box(new Point(-1, -1), new Point(-1, -1));
            }

            return new Box(min, max);
        }

        public override string ToString()
        {
            return $"(P1: {P1}, P2: {P2})";
        }
    }
}
