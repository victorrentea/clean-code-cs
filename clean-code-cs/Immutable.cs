using System;

namespace Victor.Training.Cleancode
{

    public static class ImmutableAdvanced
    {
        public static void Main()
        {
            List<int> list = new List<Int32> { 1, 2, 3 }; // ArrayList

            Immutable immutable = new Immutable(1, 2, list, new Other(15));
            Console.WriteLine("Before: " + immutable);

            Wilderness(immutable);

            Console.WriteLine("After:  " + immutable);
        }

        private static void Wilderness(Immutable immutable)
        {
            // dark, deep logic not expected to change the immutable object x,y
            immutable.GetList().Clear();
        }
    }

    // shallow immutable
    public class Immutable
    {
        private readonly int x;
        private readonly int y;
        private readonly List<int> list;
        private readonly Other other;

        public Immutable(int x, int y, List<int> list, Other other)
        {
            this.x = x;
            this.y = y;
            this.list = list;
            this.other = other;
        }

        public List<int> GetList()
        {
            return list;
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }

        public Other GetOther()
        {
            return other;
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

        public void SetA(int a)
        {
            this.a = a;
        }

        public override string ToString()
        {
            return $"Other{{a={a}}}";
        }
    }
}
