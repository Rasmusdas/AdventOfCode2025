using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var file = GetType().Name.Contains("Day") ? GetType().Name : GetType().BaseType.Name; 
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"AoC2025.Input.{file}.txt");
            
            using var reader = new StreamReader(stream);
            input = reader.ReadToEnd();
            inputLines = input.Split("\n");
        }
    }
}
