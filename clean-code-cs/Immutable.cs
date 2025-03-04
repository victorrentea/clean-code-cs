using System;
using System.Collections.Immutable;

namespace Victor.Training.Cleancode
{

    public static class ImmutableAdvanced
    {
        public static void Main()
        {
            ImmutableList<int> list = new[] { 1, 2, 3 }.ToImmutableList();

            Immutable immutable = new(1, 2, list, new Other(15));
            Console.WriteLine("Before: " + immutable);

            Wilderness(immutable);
            list.Clear();

            Console.WriteLine("After:  " + immutable);
        }

        private static void Wilderness(Immutable immutable)
        {
            // dark, deep logic not expected to change the immutable object x,y
            //immutable.GetList().Clear();
        }
    }

    // shallow immutable
    public class Immutable
    {
        public readonly int x;
        public readonly int y;
        public readonly ImmutableList<int> list; //
        public readonly Other other;

        public Immutable(int x, int y, ImmutableList<int> list, Other other)
        {
            this.x = x;
            this.y = y;
            this.list = list; // malloc = heavy
            this.other = other;
        }

        public override string ToString()
        {
            return $"Immutable{{x={x}, y={y}, numbers={string.Join(", ", list)}, other={other}}}";
        }
    }

    public class Other
    {
        private int a;

        public Other(int a)
        {
            this.a = a;
        }

        public int GetA()
        {
            return a;
        }

        

        public override string ToString()
        {
            return $"Other{{a={a}}}";
        }
    }
}
