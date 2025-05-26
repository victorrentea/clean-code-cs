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
            Console.WriteLine(new One(two).F());
            Console.WriteLine(two.G(new R(1)));
        }
    }

    class One
    {
        private readonly Two _two;

        public One(Two two)
        {
            _two = two;
        }

        public int F()
        {
            return 2 * _two.G(new R(3));
        }
    }

    class Two
    {
        public int G(R r)
        {
            int b = 2;

            BetterName(r, "value");

            return 1 + b + r.X;
        }

        private static void BetterName(R rr, string v) // NOSONAR
        {
            Console.WriteLine("b=" + rr.X + v);
        }

        public double Loop(List<int> numbers)
        {
            return Math.Sqrt(ComputeSumOfSquares(numbers));
        }

        private static double ComputeSumOfSquares(List<int> numbers)
        {
            return numbers.Where(n => n % 2 == 0)
                            .Select(n => n * n)
                            .Sum();
        }
    }

    public record R(int X);
}