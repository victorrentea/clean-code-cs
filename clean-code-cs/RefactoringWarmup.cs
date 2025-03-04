using System;
using System.Collections.Generic;
using System.Linq;

namespace Victor.Training.Cleancode
{
    class RefactoringWarmup
    {
        public static void Main(string[] args)
        {
            Two two = new Two();
            Console.WriteLine(two.Loop(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }));
            //one.Two = null;
            Console.WriteLine(new One(two).F());
            Console.WriteLine(two.G(2, new R(1)));
        }
    }


    record One(ITwo Two)
    {
        public int F()
        {
            return 2 * Two.G(1, new R(3));
        }
    }

    class Two : ITwo
    {
        //List<int> list=new List<int>();

        // @Deprecated to remove in 2025 March, I hope!
        //public int G(R r)// preserve the old sign
        //{
        //    return G(r, 1);
        //}
        public int G(int v, R r)// breaking change of your lib => lib-2.0
        {
            int bb = 2;
            Console.WriteLine("b=" + bb);
            return v + bb + r.X;
        }

        public double Loop(List<int> numbers)
        {
            const int V = 987;
            Console.WriteLine("b=" + V);
            double ssq = 0;
            foreach (var number in numbers)
            {
                if (number % 2 == 0)
                {
                    ssq += number * number;
                }
            }
            return Math.Sqrt(ssq);
        }
    }

    public record R(int X);
}

// - extract/inline constant
//    - Inline Variable 'b'
//    - Extract Variable '1', '3 * two.g()'
//    - Extract Method 'System.out..' (+replace duplicate)
//    - Inline Method 'g'
//    - Extract Parameter '1', 'r.x()'
//    - Change Signature 'g' reorder params
//    - Extract Interface 'Two'->ITwo
