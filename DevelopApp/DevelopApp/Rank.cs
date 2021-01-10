using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopApp
{
    class Rank
    {
        public Rank(string n, int min, int max)
        {
            Name = n;
            MinRating = min;
            MaxRating = max;
        }
        public string Name { get; set; }
        public int MinRating { get; set; }
        public int MaxRating { get; set; }
    }
}
