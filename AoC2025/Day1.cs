using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day1 : Day
    {

        public void SolvePart1()
        {
            int dial = 50;
            int count = 0;

            foreach(var line in inputLines)
            {
                int rotateAmount = int.Parse(line.Substring(1));
                if (line[0] == 'L')
                {
                    dial = Mod(dial - rotateAmount, 100);
                }
                else
                {
                    dial = Mod(dial + rotateAmount,100);
                }
                if(dial == 0)
                {
                    count++;
                }
            }

            Console.WriteLine(count);
            
        }


        public void SolvePart2()
        {
            int dial = 50;
            int count = 0;
            int prevCount = 0;

            foreach (var line in inputLines)
            {
                int rotateAmount = int.Parse(line.Substring(1));
                int prevDial = dial;

                count += (rotateAmount - (rotateAmount % 100)) / 100;

                rotateAmount = (rotateAmount % 100);

                if (line[0] == 'L')
                {
                    dial = Mod(dial - rotateAmount, 100);

                    if (dial > prevDial && dial != 0 && prevDial != 0)
                    {
                        count++;
                    }
                }
                else
                {
                    dial = Mod(dial + rotateAmount, 100);

                    if (dial < prevDial && dial != 0 && prevDial != 0)
                    {
                        count++;
                    }
                }

                if (dial == 0)
                {
                    count++;
                }

                prevCount = count;

            }

            Console.WriteLine(count);
        }

        public int Mod(int a,int b)
        {
            return (int)(a - b * Math.Floor(a / (double)(b)));
        }
    }
}
