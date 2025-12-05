using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day2 : Day
    {

        public void SolvePart1()
        {
            var idRanges = input.Split(",");

            double sum = 0;

            foreach(var idRange in idRanges)
            {
                var range = idRange.Split("-");

                long start = long.Parse(range[0]);
                long end = long.Parse(range[1]);


                for(long i = start; i < end; i++)
                {
                    var testString = i.ToString();
                    var string1 = testString.Substring(0, testString.Length / 2);
                    var string2 = testString.Substring(testString.Length / 2);


                    if(string1 == string2)
                    {
                        sum += i;
                    }
                }
            }
            Console.WriteLine(sum);
        }


        public void SolvePart2()
        {
            var idRanges = input.Split(",");

            double sum = 0;


            foreach (var idRange in idRanges)
            {
                var range = idRange.Split("-");

                long start = long.Parse(range[0]);
                long end = long.Parse(range[1]);


                for (long i = start; i <= end; i++)
                {
                    var testString = i.ToString();

                    if (IsRepeating(testString))
                    {
                        sum += i;
                    }
                }
            }

            Console.WriteLine(sum);
        }


        public bool IsRepeating(string testString)
        {
            int left = 0;
            int right = 1;
            int seqLength = 1;
            int curSegLength = 0;
            bool valid = false;

            while (right < testString.Length)
            {
                if (testString[left] == testString[right])
                {
                    left = (left + 1) % seqLength;
                    curSegLength = (curSegLength + 1) % seqLength;
                    valid = true;
                    right++;
                }
                else if (testString[left] != testString[right])
                {
                    left = 0;

                    seqLength = right;

                    if (testString[left] == testString[right])
                    {
                        left = (left + 1) % seqLength;
                        curSegLength = (curSegLength + 1) % seqLength;
                        valid = true;
                        right++;
                    }
                    else
                    {
                        right++;
                        seqLength = right;
                        curSegLength = 0;
                        valid = false;
                    }

                    
                }
            }

            if (valid && left == 0)
            {
                return true;
            }

            return false;
        }


        public int Mod(int a,int b)
        {
            return (int)(a - b * Math.Floor(a / (double)(b)));
        }
    }
}
