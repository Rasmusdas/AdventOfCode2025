using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AoC2025
{
    public class Day9 : Day
    {
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

            //List<Box> overlaps = new();

            
            //for (int i = 1; i < horizontalLines.Count; i++)
            //{
            //    double y = horizontalLines[i].P1.Y;

            //    Point p1N = new Point(double.MinValue, y);
            //    Point p2N = new Point(double.MaxValue, y);

            //    Line testLine = new Line(p1N, p2N);

            //    int lastIntersect = -1;

            //    for (int j = 0; j < verticalLines.Count; j++)
            //    {
            //        if (verticalLines[j].Intersects(testLine))
            //        {
            //            if (lastIntersect == -1 && Math.Min(verticalLines[j].P1.Y, verticalLines[j].P2.Y) != y)
            //            {
            //                lastIntersect = j;
            //            }
            //            else if(Math.Min(verticalLines[j].P1.Y, verticalLines[j].P2.Y) != y)
            //            {
            //                var l1 = verticalLines[lastIntersect];
            //                var l2 = verticalLines[j];

            //                var minY = Math.Max(Math.Max(Math.Min(l1.P1.Y, l1.P2.Y), Math.Min(l2.P1.Y, l2.P2.Y)), horizontalLines[i-1].P1.Y);
            //                var maxY = Math.Min(Math.Min(Math.Max(l1.P1.Y, l1.P2.Y), Math.Max(l2.P1.Y, l2.P2.Y)), horizontalLines[i].P1.Y);
            //                var minX = Math.Min(Math.Max(l1.P1.X, l1.P2.X), Math.Max(l2.P1.X, l2.P2.X));
            //                var maxX = Math.Max(Math.Min(l1.P1.X, l1.P2.X), Math.Min(l2.P1.X, l2.P2.X));
            //                overlaps.Add(new Box(new Point(minX, minY), new Point(maxX, maxY)));
            //                lastIntersect = -1;
            //            }

            //        }
            //    }
            //}



            //for (int i = 0; i < overlaps.Count; i++)
            //{
            //    for (int j = i+1; j < overlaps.Count; j++)
            //    {
            //        if (overlaps[i].GetOverlapSize(overlaps[j]) > 0)
            //        {

            //            if (overlaps[j].P1.X == overlaps[i].P1.X && overlaps[j].P2.X == overlaps[i].P2.X)
            //            {
            //                overlaps[j].P1.Y++;
            //            }
            //            else if (overlaps[i].P1.X <= overlaps[j].P1.X && overlaps[i].P2.X >= overlaps[j].P2.X)
            //            {
            //                overlaps[j].P1.Y++;
            //            }

            //            else if (overlaps[j].P1.X <= overlaps[i].P1.X && overlaps[j].P2.X >= overlaps[i].P2.X)
            //            {
            //                overlaps[i].P2.Y--;
            //            }


            //        }
            //    }  
            //}

            double bestSize = -1;
            Box bestBox = null;

            Stopwatch sw = new();
            sw.Start();
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
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
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(bestBox);

            //var grid = new char[14,14];


            //for (double i = bestBox.P1.Y; i <= bestBox.P2.Y; i++)
            //{
            //    for (double j = bestBox.P1.X; j <= bestBox.P2.X; j++)
            //    {
            //        grid[(int)i, (int)j] = (char)65;
            //    }
            //}

            //DrawGrid(grid);

            Console.WriteLine(bestSize);

            
        }
        public void DrawGrid(char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i,j] != 0)
                    {
                        Console.Write(grid[i, j]);
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
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
            if (horizontal == other.horizontal) return horizontal ? other.P1.X == P1.X : other.P1.Y == P1.Y;

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
