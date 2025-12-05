using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    public class Day
    {

        public string[] inputLines;
        public string input;
        public Day()
        {
            input = File.ReadAllText(this.GetType().Name + ".txt");
            inputLines = File.ReadAllLines(this.GetType().Name + ".txt");
        }
    }
}
